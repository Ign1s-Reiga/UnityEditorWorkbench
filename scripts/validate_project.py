#!/usr/bin/env python3
from __future__ import annotations

import json
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
REQUIRED = [
    "CLAUDE.md",
    ".claude/settings.json",
    "Packages/manifest.json",
    "Packages/com.ign1s.editor-workbench/package.json",
    "Packages/com.ign1s.editor-workbench/Editor/Ign1s.EditorWorkbench.Editor.asmdef",
    "Packages/com.ign1s.editor-workbench/Editor/Core/AssemblyInfo.cs",
    "ProjectSettings/ProjectVersion.txt",
]
PROHIBITED_DIRECTORIES = {"Library", "Temp", "Logs", "UserSettings"}


def fail(message: str, errors: list[str]) -> None:
    errors.append(message)


def parse_frontmatter(path: Path, errors: list[str]) -> dict[str, str]:
    text = path.read_text(encoding="utf-8")
    if not text.startswith("---\n"):
        fail(f"{path.relative_to(ROOT)}: missing YAML frontmatter", errors)
        return {}
    end = text.find("\n---\n", 4)
    if end < 0:
        fail(f"{path.relative_to(ROOT)}: unterminated YAML frontmatter", errors)
        return {}

    result: dict[str, str] = {}
    for line in text[4:end].splitlines():
        if not line or line.startswith(" ") or ":" not in line:
            continue
        key, value = line.split(":", 1)
        result[key.strip()] = value.strip()
    return result


def check_braces(path: Path, errors: list[str]) -> None:
    text = path.read_text(encoding="utf-8")
    stripped = re.sub(r'//.*?$|/\*.*?\*/|"(?:\\.|[^"\\])*"', '', text, flags=re.M | re.S)
    balance = 0
    for character in stripped:
        if character == "{":
            balance += 1
        elif character == "}":
            balance -= 1
            if balance < 0:
                fail(f"{path.relative_to(ROOT)}: closing brace before opening brace", errors)
                return
    if balance != 0:
        fail(f"{path.relative_to(ROOT)}: unbalanced braces ({balance})", errors)


def main() -> int:
    errors: list[str] = []

    for relative in REQUIRED:
        if not (ROOT / relative).exists():
            fail(f"Missing required file: {relative}", errors)

    for directory in PROHIBITED_DIRECTORIES:
        if (ROOT / directory).exists():
            fail(f"Generated directory must not be committed: {directory}", errors)

    for path in ROOT.rglob("*.json"):
        try:
            json.loads(path.read_text(encoding="utf-8"))
        except Exception as exception:
            fail(f"{path.relative_to(ROOT)}: invalid JSON: {exception}", errors)

    names: dict[str, Path] = {}
    for pattern, kind in ((".claude/skills/*/SKILL.md", "skill"), (".claude/agents/**/*.md", "agent")):
        for path in ROOT.glob(pattern):
            frontmatter = parse_frontmatter(path, errors)
            name = frontmatter.get("name")
            description = frontmatter.get("description")
            if not name:
                fail(f"{path.relative_to(ROOT)}: missing {kind} name", errors)
            if not description:
                fail(f"{path.relative_to(ROOT)}: missing {kind} description", errors)
            if name:
                key = f"{kind}:{name}"
                if key in names:
                    fail(f"Duplicate {kind} name {name}: {names[key]} and {path}", errors)
                names[key] = path

    compatibility_root = ROOT / "Packages/com.ign1s.editor-workbench/Editor/Compatibility"
    for path in ROOT.rglob("*.cs"):
        check_braces(path, errors)
        text = path.read_text(encoding="utf-8")
        is_compatibility = compatibility_root in path.parents
        if not is_compatibility and "UnityEditorInternal" in text:
            fail(f"{path.relative_to(ROOT)}: UnityEditorInternal is restricted to Editor/Compatibility", errors)
        if not is_compatibility and re.search(r"\b(System\.)?Reflection\b", text):
            fail(f"{path.relative_to(ROOT)}: reflection is restricted to Editor/Compatibility", errors)
        if (
            "using System;" in text
            and "using UnityEngine;" in text
            and re.search(r"\bObject\b", text)
            and "using Object = UnityEngine.Object;" not in text
        ):
            fail(f"{path.relative_to(ROOT)}: ambiguous Object type; add a UnityEngine.Object alias", errors)

    if errors:
        print("Validation failed:")
        for error in errors:
            print(f"- {error}")
        return 1

    print(f"Validation passed: {len(list(ROOT.rglob('*.cs')))} C# files, {len(names)} Claude extensions.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
