#!/usr/bin/env bash
set -euo pipefail

if [[ -z "${UNITY_EDITOR_PATH:-}" ]]; then
  echo "UNITY_EDITOR_PATH is not set." >&2
  exit 2
fi

if [[ ! -x "$UNITY_EDITOR_PATH" ]]; then
  echo "Unity executable not found or not executable: $UNITY_EDITOR_PATH" >&2
  exit 2
fi

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
RESULT_DIR="$ROOT_DIR/TestResults"
RESULT_FILE="$RESULT_DIR/editmode-results.xml"
LOG_FILE="$RESULT_DIR/unity-editmode.log"
mkdir -p "$RESULT_DIR"
rm -f "$RESULT_FILE"

# Do not add -quit. It shuts the editor down before -runTests executes, which makes
# the run exit 0 having run no tests. The test framework quits the editor itself.
"$UNITY_EDITOR_PATH" \
  -batchmode \
  -nographics \
  -projectPath "$ROOT_DIR" \
  -runTests \
  -testPlatform EditMode \
  -testResults "$RESULT_FILE" \
  -logFile "$LOG_FILE"

if [[ ! -f "$RESULT_FILE" ]]; then
  echo "Unity exited 0 but wrote no test results to $RESULT_FILE. See $LOG_FILE." >&2
  exit 1
fi

echo "EditMode results written to $RESULT_FILE"
