# Claude Code workflow

## Start a session

Run Claude Code from the repository root so the root `CLAUDE.md`, project skills, settings, and subagents are discovered.

```bash
claude
```

## Recommended first pass

1. Invoke `/plan-next-milestone M2 reliability and Unity 6000.5 verification`.
2. Ask `@unity-editor-architect` to review the resulting boundary and acceptance criteria.
3. Ask `@unity-editor-implementer` to implement one approved vertical slice.
4. Invoke `/review-editor-code` or ask `@unity-test-reviewer` for a read-only review.
5. Invoke `/run-unity-tests editmode` on a machine with `UNITY_EDITOR_PATH` configured.
6. Ask `@unity-compatibility-auditor` to audit any Unity-version-specific changes.

## Example implementation request

```text
Use @unity-editor-architect to plan drag-and-drop inspector tabs without reflection.
After the plan, use @unity-editor-implementer to implement the smallest complete slice,
add EditMode tests, update docs, run static validation, and report what still needs Unity verification.
```

Keep each change bounded. Do not ask one agent to implement the entire commercial-asset feature set in one pass.
