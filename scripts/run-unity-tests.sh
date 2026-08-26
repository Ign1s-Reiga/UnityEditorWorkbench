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
mkdir -p "$RESULT_DIR"

"$UNITY_EDITOR_PATH" \
  -batchmode \
  -nographics \
  -quit \
  -projectPath "$ROOT_DIR" \
  -runTests \
  -testPlatform EditMode \
  -testResults "$RESULT_DIR/editmode-results.xml" \
  -logFile "$RESULT_DIR/unity-editmode.log"
