# Development Setup Guide

## Prerequisites for Local Development

### Required Software
- **.NET 8.0 SDK** or later
- **Docker Desktop** (for PostgreSQL container)
- **Visual Studio 2026 Insiders** (recommended) or **Visual Studio Code**

### .NET Aspire Workload Installation

```bash
# Install Aspire workload
dotnet workload install aspire

# Verify installation
dotnet workload list
```

## Project Setup

### 1. Clone and Setup
```bash
git clone <repository-url>
cd vs2026-dev-productivity
dotnet restore
```

### 2. Environment Configuration

Create `src/Aspire.Host/appsettings.Development.json` for local overrides:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Warning",
      "Aspire": "Information"
    }
  }
}
```

## Running the Application

### Option 1: Visual Studio 2026 Insiders
1. Open `VS2026DevProductivity.sln`
2. Set `Aspire.Host` as startup project
3. Press F5 to run

### Option 2: Command Line
```bash
cd src/Aspire.Host
dotnet run
```

### Option 3: Individual Services (for debugging)
```bash
# Terminal 1 - PostgreSQL (if not using Docker)
# Use local PostgreSQL or Docker container

# Terminal 2 - MinimalApi
cd src/Aspire.MinimalApi
dotnet run

# Terminal 3 - BlazorApp  
cd src/Aspire.BlazorApp
dotnet run

# Terminal 4 - ApiGateway
cd src/Aspire.ApiGateway
dotnet run
```

## Troubleshooting

### Common Issues

1. **Docker not starting**
   ```bash
   # Check Docker status
   docker --version
   docker ps
   ```

2. **Port conflicts**
   - Check `launchSettings.json` in each project
   - Default ports: Host(15888), API(5001), Gateway(5000), Blazor(5002)

3. **Database connection issues**
   - Ensure PostgreSQL container is running
   - Check connection strings in Aspire configuration

4. **Package restore issues**
   ```bash
   dotnet clean
   dotnet restore --force
   dotnet build
   ```

### Debugging Tips

1. **Enable detailed logging**
   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Debug",
         "Aspire": "Debug"
       }
     }
   }
   ```

2. **Check service health**
   - Visit `/health` endpoint on each service
   - Monitor Aspire Dashboard for service status

3. **Database debugging**
   ```bash
   # Connect to PostgreSQL container
   docker exec -it <postgres-container> psql -U postgres -d productdb
   
   # List tables
   \dt
   
   # Query products
   SELECT * FROM "Products";
   ```

## IDE Setup

### Visual Studio 2026 Insiders
- Install latest preview version
- Enable Aspire project templates
- Configure Docker integration

### Visual Studio Code
Required extensions:
- C# Dev Kit
- .NET Aspire (when available)
- Docker
- REST Client (for API testing)

## Performance Optimization

### Development Mode
```bash
# Faster startup - skip migrations
export ASPNETCORE_ENVIRONMENT=Development
export SKIP_MIGRATIONS=true
```

### Production Considerations
- Use Redis for caching
- Configure proper logging levels
- Implement health checks monitoring
- Set up proper secrets management