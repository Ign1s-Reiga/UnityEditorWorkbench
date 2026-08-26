# Unity Editor Workbench

## Mission

Build a stable, maintainable Unity Editor productivity package that covers the workflows we actually use instead of cloning commercial assets feature-for-feature.

## Target environment

- Unity: 6000.5
- Language: C#
- Package: `Packages/com.ign1s.editor-workbench`
- Scope: Editor-only. Do not add runtime assemblies unless a feature strictly requires them.
- UI: IMGUI for the MVP. UI Toolkit may be introduced only through an explicit migration decision.

## Architecture

- `Editor/Core`: settings, persistence, identifiers, shared utilities
- `Editor/ProjectBrowser`: folder decoration and favorites
- `Editor/Inspector`: tabbed inspector, history, target persistence
- `Editor/Hierarchy`: hierarchy row decorations
- `Editor/Settings`: Project Settings integration
- `Editor/Compatibility`: the only allowed location for version-specific or undocumented API access
- `Tests/Editor`: EditMode tests

Dependency direction:

```text
Feature modules -> Core
Settings -> Core and feature entry points
Core -> UnityEditor / UnityEngine only
```

Feature modules must not depend on one another unless the dependency is represented by a small interface in `Core`.

## Non-negotiable rules

1. Prefer documented public Unity APIs.
2. Do not use reflection, `UnityEditorInternal`, or copied Unity source outside `Editor/Compatibility`.
3. Do not modify Unity's built-in Inspector or Project Browser through reflection. Build companion windows and documented callbacks.
4. Store shared project configuration in `ProjectSettings` and personal state in `UserSettings`.
5. Persist assets and folders by GUID. Persist scene objects with `GlobalObjectId` plus a direct serialized reference when appropriate.
6. Every event subscription must have a matching unsubscribe path unless the subscriber is an `[InitializeOnLoad]` static service designed for the editor lifetime.
7. Destroy cached `UnityEditor.Editor` instances during disable and assembly reload.
8. All user-visible mutations must support Undo when Unity objects are changed.
9. Avoid allocations in Project and Hierarchy row callbacks. Perform work only during relevant IMGUI events.
10. Never hand-edit generated `.csproj`, `.sln`, `Library`, or `UserSettings` content.

## Coding standards

- Use file-scoped namespaces.
- Use four spaces and braces on new lines.
- Prefer `internal` unless a type is part of the package extension API.
- Keep EditorWindow rendering methods small; extract state transitions and persistence logic.
- Avoid LINQ in hot GUI callbacks.
- Use explicit names; do not abbreviate `selection`, `appearance`, `identifier`, or `settings`.
- Add XML documentation only for public extension points.
- Do not introduce nullable annotations until the Unity compiler configuration enables them repository-wide.

## Workflow

Before implementation:

1. Read `docs/ARCHITECTURE.md` and the relevant feature spec.
2. Identify the public Unity APIs required.
3. State persistence behavior, Undo behavior, and domain-reload behavior.
4. Add or update EditMode tests for non-visual logic.

After implementation:

1. Run `python scripts/validate_project.py`.
2. Run EditMode tests when a Unity executable is available.
3. Review the diff for allocations in GUI callbacks and leaked event subscriptions.
4. Update `docs/ROADMAP.md` and `CHANGELOG.md` when behavior changes.

## Commands

Repository validation:

```bash
python scripts/validate_project.py
```

Unity EditMode tests:

```bash
UNITY_EDITOR_PATH=/path/to/Unity ./scripts/run-unity-tests.sh
```

Windows PowerShell:

```powershell
$env:UNITY_EDITOR_PATH = "C:\\Program Files\\Unity\\Hub\\Editor\\6000.5.2f1\\Editor\\Unity.exe"
./scripts/run-unity-tests.ps1
```

## Definition of done

A feature is complete only when:

- It compiles in Unity 6.5.
- Core non-visual behavior has EditMode coverage.
- Domain reload does not duplicate callbacks or leak editors.
- Missing or deleted targets fail gracefully.
- Project settings and user settings are stored in the correct scope.
- Documentation records any compatibility assumptions.
