# Unity compatibility

## Baseline

The project baseline is Unity `6000.3.14f1`, within Unity 6.3 LTS. The package declares `unity: 6000.3`.

## Public APIs used

- `EditorApplication.projectWindowItemOnGUI`
- `EditorApplication.hierarchyWindowItemOnGUI`
- `EditorApplication.RepaintProjectWindow`
- `EditorApplication.RepaintHierarchyWindow`
- `Selection.selectionChanged`
- `Editor.CreateEditor`
- `ScriptableSingleton<T>` and `FilePathAttribute`
- `AssetDatabase` GUID/path conversion
- `GlobalObjectId`
- `SettingsProvider`
- `Undo.RecordObject`

## Version-specific concerns

The hierarchy callback may evolve in later Unity 6 releases. Do not spread preprocessor checks across feature code. Add an adapter under `Editor/Compatibility` when upgrading.

The MVP intentionally avoids built-in window toolbar injection and built-in Inspector reflection. Toolbar integration is deferred until a focused milestone verifies a documented public API and fallback behavior for every supported Unity version.
