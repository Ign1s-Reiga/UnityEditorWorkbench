---
name: review-editor-code
description: Review Unity Editor Workbench changes for correctness, lifecycle safety, serialization, API compatibility, Undo support, performance, and tests. Use after code changes or before a merge.
allowed-tools: Read Grep Glob Bash(git diff *) Bash(git status *)
---

Review the current changes without editing files.

## Checklist

- Compile-time API correctness for Unity 6000.5 LTS
- Matching subscribe and unsubscribe behavior
- Cached `UnityEditor.Editor` destruction
- Safe behavior after domain reload and target deletion
- GUID and `GlobalObjectId` persistence correctness
- ProjectSettings versus UserSettings scope
- Undo and dirty-state handling
- No reflection or `UnityEditorInternal` outside `Editor/Compatibility`
- No avoidable allocations in Project or Hierarchy row callbacks
- No unconditional repaint loops
- EditMode coverage for non-visual logic
- Documentation and changelog consistency

Report findings by severity: blocker, major, minor, note. Include file and line references, evidence, and a concrete fix direction. State explicitly when no actionable findings are present.
