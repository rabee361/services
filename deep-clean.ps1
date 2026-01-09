# 🧹 Glow Shop - Deep Clean Script
# This script removes all build artifacts and temporary files to make the project folder as small as possible.

Write-Host "🧼 Starting Deep Clean..." -ForegroundColor Cyan

$targets = @(
    "bin",
    "obj",
    ".vs",
    "publish_executables",
    "TestResults"
)

foreach ($target in $targets) {
    Write-Host "🔍 Searching for $target folders..." -ForegroundColor Gray
    Get-ChildItem -Path . -Filter $target -Recurse -Force -ErrorAction SilentlyContinue | ForEach-Object {
        Write-Host "🗑️  Deleting: $($_.FullName)" -ForegroundColor Yellow
        Remove-Item -Path $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
    }
}

Write-Host "`n✨ Clean complete! Your project folder is now source-code only." -ForegroundColor Green
Write-Host "You can now zip the 'micro-services' folder and it should be very small (likely < 20MB)."
