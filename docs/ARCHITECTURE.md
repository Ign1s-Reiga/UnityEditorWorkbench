# Architecture

## Product boundary

Editor Workbench is an Editor-only embedded UPM package. The host Unity project is a sandbox for development and tests. The package must remain copyable into another Unity project without relying on files under `Assets`.

## Modules

### Core

Owns settings, serializable state, GUID helpers, object references, constants, and shared styles. Core must not call feature windows directly.

### Project Browser

Uses `EditorApplication.projectWindowItemOnGUI` to draw lightweight overlays. Folder metadata is keyed by asset GUID so rename and move operations retain appearance.

Project-shared appearance data is stored under `ProjectSettings`. Favorites are per-user and stored under `UserSettings`.

### Inspector

A companion `EditorWindow` follows `Selection.activeObject` and presents multiple tabs. For a `GameObject`, it creates an editor for the object and each valid component so registered custom editors are reused. Other targets receive one matching editor. It does not modify Unity's built-in Inspector.

The active editor instances are ephemeral. Serialized tab state retains a direct Unity object reference and a `GlobalObjectId` string where available. Editors are destroyed on target changes, disable, and assembly reload.

### Hierarchy

Uses the documented hierarchy row callback for small overlays and controls. The callback must avoid LINQ, component enumeration, AssetDatabase scans, and persistent allocation.

### Settings

A `SettingsProvider` exposes package toggles and navigation to feature windows. Changes are saved immediately to project-scoped settings.

## Persistence

| Data | Scope | Storage |
|---|---|---|
| Feature enable flags | Project | `ProjectSettings/Ign1s.EditorWorkbench.asset` |
| Folder colors and icon names | Project | `ProjectSettings/Ign1s.EditorWorkbench.asset` |
| Favorite folders | User | `UserSettings/Ign1s.EditorWorkbench.user.asset` |
| Open inspector tabs | Window serialization | Editor layout/session |
| Future cross-session tab restore | User | Not implemented in MVP |

## Compatibility policy

Public APIs are the default. Any unavoidable version bridge must be isolated in `Editor/Compatibility`, wrapped by a small interface, guarded with Unity version defines, and have a no-op or reduced-function fallback.

The package does not use reflection to inject controls into built-in Unity windows.
