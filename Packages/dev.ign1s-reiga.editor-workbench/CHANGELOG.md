# Changelog

## 0.2.0

- Renamed the package from `com.ign1s.editor-workbench` to
  `dev.ign1s-reiga.editor-workbench`, moving the embedded directory to match.
  Package Manager treats this as a different package, so installs of the old
  name will not upgrade and must be replaced.
- Retargeted the Unity baseline from `6000.3.14f1` to `6000.5.2f1`.
- Migrated the hierarchy decorator to the Unity 6.5 `EntityId` callbacks:
  `hierarchyWindowItemByEntityIdOnGUI` and `EditorUtility.EntityIdToObject`. The
  previous `hierarchyWindowItemOnGUI` and `InstanceIDToObject` forms are obsolete as
  errors in 6.5.
- Added `-langversion:10` response files so file-scoped namespaces compile. Unity 6.5
  defaults editor assemblies to C# 9.
- Added `Editor/Compatibility` as the designated location for version-specific adapters.
- Fixed both EditMode test runners, which passed `-quit` alongside `-runTests`. Unity
  shut down before the tests ran and the scripts reported success having run none. They
  now also fail when Unity produces no results file, and report pass/fail counts.
- Fixed the Windows EditMode test runner so it waits for the Unity process and reports
  its exit code. `Unity.exe` is a GUI-subsystem binary, so the call operator returned
  immediately and left `$LASTEXITCODE` unset.
- Fixed `scripts/validate_project.py`, which walked `Library/` and failed against Unity's
  own package sources. Walks now skip generated directories, and the
  must-not-be-committed check consults git rather than testing for existence on disk.

## 0.1.0

- Added project-shared folder tint and icon metadata.
- Added per-user folder favorites.
- Added tabbed companion inspector with selection history and locking.
- Added hierarchy active-state and prefab indicators.
- Added Project Settings integration.
- Added initial EditMode tests.
