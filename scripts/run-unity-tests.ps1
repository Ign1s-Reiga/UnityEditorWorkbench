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

$LogFile = Join-Path $ResultDirectory "unity-editmode.log"
$ResultFile = Join-Path $ResultDirectory "editmode-results.xml"

# Drop any stale result so the verification below cannot pass on a previous run.
Remove-Item -LiteralPath $ResultFile -ErrorAction SilentlyContinue

# Quote path arguments so directories containing spaces survive argument parsing.
$Arguments = @(
    "-batchmode",
    "-nographics",
    "-projectPath", "`"$RootDirectory`"",
    "-runTests",
    "-testPlatform", "EditMode",
    "-testResults", "`"$ResultFile`"",
    "-logFile", "`"$LogFile`""
)

# Do not add -quit. It shuts the editor down before -runTests executes, which makes
# the run exit 0 having run no tests. The test framework quits the editor itself.
# Unity.exe is a GUI-subsystem binary. The call operator does not wait for it and
# leaves $LASTEXITCODE unset, so start it explicitly and wait for the process.
$Process = Start-Process -FilePath $env:UNITY_EDITOR_PATH -ArgumentList $Arguments -NoNewWindow -PassThru -Wait

if ($null -eq $Process) {
    throw "Unity failed to start: $env:UNITY_EDITOR_PATH"
}

if ($Process.ExitCode -ne 0) {
    throw "Unity EditMode tests failed with exit code $($Process.ExitCode). See $LogFile."
}

if (-not (Test-Path -LiteralPath $ResultFile)) {
    throw "Unity exited 0 but wrote no test results to $ResultFile. See $LogFile."
}

[xml]$Results = Get-Content -LiteralPath $ResultFile
$Run = $Results.SelectSingleNode("/test-run")
Write-Output "EditMode tests: $($Run.total) total, $($Run.passed) passed, $($Run.failed) failed, $($Run.skipped) skipped."

if ([int]$Run.failed -gt 0) {
    throw "$($Run.failed) EditMode test(s) failed. See $ResultFile."
}
