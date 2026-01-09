# 🚀 Glow Shop - Build Executables Script
# This script builds all microservices and web portals into standalone executables.

$projects = @(
    "src/Users.Api",
    "src/Product.Api",
    "src/Inventory.Api",
    "src/Order.Api",
    "src/ApiGateway",
    "src/Ecommerce.Web",
    "src/Ecommerce.Admin"
)

$outputRoot = "publish_executables"

if (Test-Path $outputRoot) {
    Write-Host "🧹 Cleaning old publish folder..." -ForegroundColor Yellow
    Remove-Item -Path $outputRoot -Recurse -Force
}

New-Item -ItemType Directory -Path $outputRoot | Out-Null

foreach ($projectPath in $projects) {
    $projectName = Split-Path $projectPath -Leaf
    Write-Host "🏗️  Building $projectName..." -ForegroundColor Cyan
    
    # Construct publish command
    # -r win-x64: Target Windows 64-bit
    # -c Release: Optimized build
    # --self-contained: Include .NET runtime
    # PublishSingleFile: Combine into one .exe
    dotnet publish $projectPath -c Release -r win-x64 --self-contained true `
        /p:PublishSingleFile=true `
        /p:IncludeNativeLibrariesForSelfExtract=true `
        -o "$outputRoot/$projectName"
        
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ $projectName build successfully!" -ForegroundColor Green
    } else {
        Write-Host "❌ Failed to build $projectName" -ForegroundColor Red
    }
}

Write-Host "`n🎉 All builds completed! Check the '$outputRoot' folder." -ForegroundColor DarkCyan
Write-Host "Note: For Web and Admin projects, make sure to copy 'wwwroot' and 'appsettings.json' if they aren't bundled."
