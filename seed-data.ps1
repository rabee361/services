param (
    [string]$MySqlHost = "localhost",
    [string]$MySqlPort = "3310",
    [string]$MySqlUser = "root",
    [string]$MySqlPassword = "password"
)

$SqlFile = Join-Path $PSScriptRoot "seed-data.sql"

if (-not (Test-Path $SqlFile)) {
    Write-Host "❌ Error: seed-data.sql not found!" -ForegroundColor Red
    exit 1
}

Write-Host "🚀 Seeding data to MySQL at $MySqlHost:$MySqlPort..." -ForegroundColor Cyan

# Attempt to execute using mysql CLI
try {
    # We use --local-infile=1 if needed, but for simple inserts it's not required.
    # We pipe the content to allow complex SQL structures if mysql -e has issues with multiple lines.
    Get-Content $SqlFile | & mysql --host=$MySqlHost --port=$MySqlPort --user=$MySqlUser --password=$MySqlPassword --force
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Data seeded successfully!" -ForegroundColor Green
    } else {
        Write-Host "⚠️ MySQL returned an error. Make sure the database is running and credentials are correct." -ForegroundColor Yellow
    }
} catch {
    Write-Host "❌ Failed to execute mysql command. Is 'mysql' in your PATH?" -ForegroundColor Red
    Write-Host "Details: $_"
}
