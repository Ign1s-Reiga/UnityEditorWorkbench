$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($env:UNITY_EDITOR_PATH)) {
    throw "UNITY_EDITOR_PATH is not set."
}

if (-not (Test-Path -LiteralPath $env:UNITY_EDITOR_PATH)) {
    throw "Unity executable was not found: $env:UNITY_EDITOR_PATH"
}

$RootDirectory = Split-Path -Parent $PSScriptRoot
$ResultDirectory = Join-Path $RootDirectory "TestResults"
New-Item -ItemType Directory -Force -Path $ResultDirectory | Out-Null

$Arguments = @(
    "-batchmode",
    "-nographics",
    "-quit",
    "-projectPath", $RootDirectory,
    "-runTests",
    "-testPlatform", "EditMode",
    "-testResults", (Join-Path $ResultDirectory "editmode-results.xml"),
    "-logFile", (Join-Path $ResultDirectory "unity-editmode.log")
)

& $env:UNITY_EDITOR_PATH @Arguments
if ($LASTEXITCODE -ne 0) {
    throw "Unity EditMode tests failed with exit code $LASTEXITCODE."
}
