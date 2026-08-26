---
name: run-unity-tests
description: Run Unity Editor Workbench validation and Unity EditMode tests, then summarize failures without hiding incomplete verification.
argument-hint: "[editmode]"
disable-model-invocation: true
allowed-tools: Read Bash(python scripts/validate_project.py) Bash(./scripts/run-unity-tests.sh *) Bash(powershell -File scripts/run-unity-tests.ps1 *)
---

Validate the repository, then run Unity EditMode tests.

1. Run `python scripts/validate_project.py`.
2. Check whether `UNITY_EDITOR_PATH` exists.
3. On Windows, run `scripts/run-unity-tests.ps1`; otherwise run `scripts/run-unity-tests.sh`.
4. Read `TestResults/editmode-results.xml` and `TestResults/unity-editmode.log` when generated.
5. Report the exact failing test names and first useful error for each failure.
6. If Unity is unavailable, say that only static repository validation ran. Never report the test suite as passing in that case.
