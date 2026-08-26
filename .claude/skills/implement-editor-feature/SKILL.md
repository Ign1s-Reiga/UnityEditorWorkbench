---
name: implement-editor-feature
description: Implement a bounded Unity Editor Workbench feature with public APIs, lifecycle-safe code, persistence rules, tests, and documentation. Use for feature requests inside the embedded package.
argument-hint: <feature and acceptance criteria>
---

Implement `$ARGUMENTS` in the Unity Editor Workbench package.

## Procedure

1. Read `CLAUDE.md`, `docs/ARCHITECTURE.md`, and `docs/FEATURES.md`.
2. Inspect existing code before creating new abstractions.
3. Write a short implementation plan covering:
   - Public Unity APIs
   - Shared versus personal persistence
   - Domain reload and deleted-target behavior
   - Undo behavior
   - Tests
4. Implement the smallest complete vertical slice.
5. Add EditMode tests for non-visual state transitions and persistence helpers.
6. Run `python scripts/validate_project.py`.
7. Run Unity EditMode tests when `UNITY_EDITOR_PATH` is available.
8. Review the diff for leaked callbacks, undisposed editors, hot-path allocations, and internal API use.
9. Update the roadmap or changelog when user-visible behavior changes.

## Constraints

- Do not edit generated Unity solution files.
- Do not add runtime code for an Editor-only feature.
- Do not use reflection or `UnityEditorInternal` outside `Editor/Compatibility`.
- Do not state that Unity compilation passed without a successful batch-mode result.
