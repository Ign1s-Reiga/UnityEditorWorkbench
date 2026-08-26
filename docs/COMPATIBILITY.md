# Unity compatibility

## Baseline

The project baseline is Unity `6000.5.2f1`, within Unity 6.5. The package declares `unity: 6000.5`.

## Language version

Unity 6.5 compiles editor assemblies as C# 9 by default, but the coding standard in
`CLAUDE.md` requires file-scoped namespaces, which is a C# 10 feature. Each assembly
therefore ships a `csc.rsp` next to its `.asmdef` containing `-langversion:10`:

- `Editor/csc.rsp`
- `Tests/Editor/csc.rsp`

Removing either file reintroduces `CS8773` on every source file in that assembly.
Raising the language version above 10 is not required and is not tested.

## Public APIs used

- `EditorApplication.projectWindowItemOnGUI`
- `EditorApplication.hierarchyWindowItemByEntityIdOnGUI`
- `EditorApplication.RepaintProjectWindow`
- `EditorApplication.RepaintHierarchyWindow`
- `EditorUtility.EntityIdToObject`
- `Selection.selectionChanged`
- `Editor.CreateEditor`
- `ScriptableSingleton<T>` and `FilePathAttribute`
- `AssetDatabase` GUID/path conversion
- `GlobalObjectId`
- `SettingsProvider`
- `Undo.RecordObject`

## Version-specific concerns

Unity 6.5 is migrating hierarchy and project callbacks from `int` instance identifiers
to `UnityEngine.EntityId`. The package uses the `EntityId` forms directly because it
targets a single baseline:

- `EditorApplication.hierarchyWindowItemOnGUI` is obsolete as an error. Use
  `hierarchyWindowItemByEntityIdOnGUI`.
- `EditorUtility.InstanceIDToObject(int)` is obsolete as an error. Use
  `EditorUtility.EntityIdToObject(EntityId)`.
- `EditorApplication.projectWindowItemOnGUI` is not obsolete in 6.5 and is still used
  as-is. Its sibling `projectWindowItemInstanceOnGUI` is obsolete; do not adopt it.

`EntityId` converts implicitly to and from `int` in both directions, so an adapter under
`Editor/Compatibility` is only needed if the package is ever required to build against a
Unity version that predates the `EntityId` callbacks. Supporting a second baseline is the
trigger for adding that adapter, not the migration itself.

The MVP intentionally avoids built-in window toolbar injection and built-in Inspector reflection. Toolbar integration is deferred until a focused milestone verifies a documented public API and fallback behavior for every supported Unity version.
