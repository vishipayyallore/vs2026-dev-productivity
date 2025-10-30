#!/usr/bin/env pwsh
# VS2026 Development Database Management Script

param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("up", "down", "restart", "logs", "status", "clean")]
    [string]$Action = "up",
    
    [Parameter(Mandatory = $false)]
    [switch]$Detach = $false
)

function Write-Info($message) {
    Write-Host "ℹ️  $message" -ForegroundColor Cyan
}

function Write-Success($message) {
    Write-Host "✅ $message" -ForegroundColor Green
}

function Write-Warning($message) {
    Write-Host "⚠️  $message" -ForegroundColor Yellow
}

function Write-Error($message) {
    Write-Host "❌ $message" -ForegroundColor Red
}

# Ensure we're in the right directory
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptDir

switch ($Action) {
    "up" {
        Write-Info "Starting VS2026 development database stack..."
        if ($Detach) {
            docker-compose up -d
        }
        else {
            Write-Info "Starting in foreground mode. Press Ctrl+C to stop."
            Write-Info "Use './dev-db.ps1 up -Detach' to run in background."
            docker-compose up
        }
        
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Database stack started successfully!"
            Write-Info "PostgreSQL: localhost:5432 (user: postgres, password: postgres, db: productdb)"
            Write-Info "pgAdmin: http://localhost:8080 (email: admin@example.com, password: admin)"
        }
    }
    
    "down" {
        Write-Info "Stopping VS2026 development database stack..."
        docker-compose down
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Database stack stopped successfully!"
        }
    }
    
    "restart" {
        Write-Info "Restarting VS2026 development database stack..."
        docker-compose restart
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Database stack restarted successfully!"
        }
    }
    
    "logs" {
        Write-Info "Showing logs for VS2026 development database stack..."
        docker-compose logs -f
    }
    
    "status" {
        Write-Info "Status of VS2026 development database stack:"
        docker-compose ps
        Write-Info ""
        Write-Info "Health check:"
        docker-compose exec postgres pg_isready -U postgres -d productdb
    }
    
    "clean" {
        Write-Warning "This will remove all containers, volumes, and data. Are you sure? (y/N)"
        $confirm = Read-Host
        if ($confirm -eq "y" -or $confirm -eq "Y") {
            Write-Info "Cleaning up VS2026 development database stack..."
            docker-compose down -v --remove-orphans
            docker volume prune -f
            Write-Success "Cleanup completed!"
        }
        else {
            Write-Info "Cleanup cancelled."
        }
    }
}