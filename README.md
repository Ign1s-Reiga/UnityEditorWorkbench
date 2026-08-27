# Unity Editor Workbench

A source-first Unity 6.5 LTS project for building a personal replacement for common productivity assets such as folder decorators and tabbed inspector tools.

The repository contains:

- An embedded UPM package: `dev.ign1s-reiga.editor-workbench`
- Folder colors, folder icons, and per-user favorites
- A tabbed inspector window with selection following, locking, and history
- Lightweight hierarchy decorations
- Editor tests
- Claude Code project instructions, skills, and specialized subagents

## Requirements

- Unity `6000.5.2f1` or another compatible Unity 6.5 LTS patch
- A current Claude Code release with project skills and custom subagents
- Python 3.10+ for repository validation

## Install the package

Install into another Unity project through Package Manager.

From a release tarball:

1. Download `dev.ign1s-reiga.editor-workbench-<version>.tgz` from the
   [releases page](https://github.com/Ign1s-Reiga/UnityEditorWorkbench/releases).
2. In Package Manager choose `Add package from tarball` and select the file.

From this repository, choose `Add package from git URL` and enter:

```text
https://github.com/Ign1s-Reiga/UnityEditorWorkbench.git?path=Packages/dev.ign1s-reiga.editor-workbench
```

Append `#<tag>` to the git URL to pin a release instead of tracking `main`.

Tags up to and including `v0.1.0` predate the package rename and live under the old
path `Packages/com.ign1s.editor-workbench`, so pin those with the old path or use the
release tarball instead.

## Open the project

1. Extract the archive.
2. Add the extracted `UnityEditorWorkbench` directory in Unity Hub.
3. Open it with Unity 6.5 LTS.
4. Wait for package resolution and script compilation.
5. Open `Window > Editor Workbench > Inspector`.

## Claude Code

Create a Git baseline after extracting the project so Claude Code can review precise diffs:

```bash
git init
git add .
git commit -m "Initial Unity Editor Workbench scaffold"
```

Then run Claude Code from the repository root:

```bash
claude
```

Useful skills:

```text
/implement-editor-feature <feature>
/review-editor-code
/run-unity-tests editmode
/plan-next-milestone
```

Specialized project agents are stored in `.claude/agents/` and can be invoked by name or delegated to automatically. See `docs/CLAUDE_WORKFLOW.md` for the recommended sequence.

## Repository validation

```bash
python scripts/validate_project.py
```

Unity EditMode tests can be launched with either script after setting `UNITY_EDITOR_PATH`:

```powershell
./scripts/run-unity-tests.ps1
```

```bash
./scripts/run-unity-tests.sh
```

## Status

This is a functional MVP scaffold, not a complete clone of any commercial asset. Public Unity Editor APIs are preferred. Reflection and undocumented Unity internals are prohibited outside a future, explicitly isolated compatibility layer.
