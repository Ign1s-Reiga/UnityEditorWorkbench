---
name: plan-next-milestone
description: Plan the next bounded milestone for Unity Editor Workbench from the roadmap, with architecture, risks, tests, and acceptance criteria. Use for planning before implementation.
argument-hint: <goal or roadmap item>
---

Plan `$ARGUMENTS` as one reviewable milestone.

Read `docs/ARCHITECTURE.md`, `docs/FEATURES.md`, `docs/ROADMAP.md`, and relevant source files. Produce:

1. User-visible outcome
2. Explicit non-goals
3. Public Unity APIs involved
4. Files and types to add or change
5. State and persistence model
6. Domain reload, scene, asset move, and deletion behavior
7. Performance risks
8. EditMode and manual test matrix
9. Acceptance criteria
10. Follow-up work intentionally deferred

Prefer a vertical slice that can be completed and verified independently.
