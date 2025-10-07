# Development Database Setup

This directory contains Docker Compose configuration for the VS2026 development environment.

## Quick Start

1. **Start the database stack:**

   ```powershell
   # Foreground mode (logs visible, Ctrl+C to stop)
   .\dev-db.ps1 up
   
   # Background mode (detached)
   .\dev-db.ps1 up -Detach
   ```

2. **Access services:**
   - **PostgreSQL**: `localhost:5432`
     - Database: `productdb`
     - Username: `postgres`
     - Password: `postgres`
   - **pgAdmin**: <http://localhost:8080>
     - Email: `admin@example.com`
     - Password: `admin`

3. **Stop the stack:**

   ```powershell
   .\dev-db.ps1 down
   ```

## Available Commands

```powershell
# Start services
.\dev-db.ps1 up           # Foreground mode
.\dev-db.ps1 up -Detach   # Background mode

# Stop services
.\dev-db.ps1 down

# Restart services
.\dev-db.ps1 restart

# View logs
.\dev-db.ps1 logs

# Check status
.\dev-db.ps1 status

# Clean up (removes all data!)
.\dev-db.ps1 clean
```

## Direct Docker Compose Commands

If you prefer using Docker Compose directly:

```bash
# Start in background
docker-compose up -d

# Stop
docker-compose down

# View logs
docker-compose logs -f

# Check status
docker-compose ps
```

## Development Workflow

1. Start the database: `.\dev-db.ps1 up -Detach`
2. Run EF Core migrations: `dotnet ef database update -p src\AspireApp.MinimalApi`
3. Start your application: `dotnet run --project src\AspireApp.MinimalApi`
4. Access pgAdmin at <http://localhost:8080> for database management

## Data Persistence

- Database data is persisted in Docker volume `vs2026-dev-productivity_postgres_data`
- pgAdmin settings are persisted in Docker volume `vs2026-dev-productivity_pgadmin_data`
- Data survives container restarts unless you use the `clean` command

## Network

- Services communicate via the `aspire-network` bridge network
- PostgreSQL is accessible from the host at `localhost:5432`
- pgAdmin is accessible from the host at `localhost:8080`
