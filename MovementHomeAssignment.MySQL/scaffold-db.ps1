# Scaffold Database Script
# This script regenerates the EF Core models and DbContext from the existing MySQL database

Write-Host "Building Infrastructure project..." -ForegroundColor Yellow
dotnet build

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed. Exiting." -ForegroundColor Red
    exit 1
}

Write-Host "Scaffolding database..." -ForegroundColor Yellow
dotnet ef dbcontext scaffold `
    "Server=localhost;Port=3306;Database=MovementHomeAssignmentDB;Uid=appuser;Pwd=apppassword;" `
    MySQL.EntityFrameworkCore `
    --output-dir Data/Models `
    --context-dir Data `
    --context ApplicationDbContext `
    --namespace "MovementHomeAssignment.Infrastructure.Data" `
    --context-namespace "MovementHomeAssignment.Infrastructure.Data" `
    --force `
    --no-onconfiguring

if ($LASTEXITCODE -eq 0) {
    Write-Host "Scaffold completed successfully." -ForegroundColor Green
    Write-Host "Models: MovementHomeAssignment.Infrastructure\Data\Models" -ForegroundColor Green
    Write-Host "DbContext: MovementHomeAssignment.Infrastructure\Data\ApplicationDbContext.cs" -ForegroundColor Green
} else {
    Write-Host "Scaffold failed." -ForegroundColor Red
    exit 1
}