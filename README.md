# VS 2026 Developer Productivity Samples 🚀

This repo showcases **.NET Aspire + Visual Studio 2026 Insiders features** to maximize developer productivity. Comprehensive .NET Aspire starter project and samples demonstrating Visual Studio 2026 Insiders features for building microservice-based cloud-native applications.

## 🚀 .NET Aspire Starter Project

A comprehensive .NET Aspire application demonstrating modern cloud-native development patterns with:

- **Orchestrated Services**: Multiple microservices managed by Aspire Host
- **API Gateway**: YARP reverse proxy for routing
- **Real-time Dashboard**: Blazor Server app for monitoring
- **Database Integration**: PostgreSQL with Entity Framework Core
- **Observability**: Built-in logging, metrics, and health checks
- **Service Discovery**: Automatic service registration and discovery

## 📁 Project Structure

```text
src/
├── Aspire.Host/              # 🎯 Orchestration and service management
├── Aspire.ServiceDefaults/   # ⚙️  Common service configurations
├── Aspire.Shared/            # 📦 Shared DTOs and models
├── Aspire.MinimalApi/        # 🔗 REST API with product endpoints
├── Aspire.BlazorApp/         # 📊 Interactive dashboard
└── Aspire.ApiGateway/        # 🌐 YARP reverse proxy
```

## 🛠️ Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for PostgreSQL)
- [Visual Studio 2026 Insiders](https://visualstudio.microsoft.com/vs/preview/) or [Visual Studio Code](https://code.visualstudio.com/)
- **.NET Aspire workload**: `dotnet workload install aspire`

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/vishipayyallore/vs2026-dev-productivity.git
cd vs2026-dev-productivity
```

## What this repo contains

- A complete Aspire-based starter app with multiple services (Minimal API, Blazor dashboard, API Gateway)
- Solution file: `VS2026DevProductivity.sln`
- Documentation: `API.md`, `SETUP.md`, and this `README.md`

## Tech stack

- .NET 8 / .NET Aspire
- YARP (API Gateway)
- Blazor (dashboard)
- EF Core + PostgreSQL
- OpenTelemetry + Serilog

## Prerequisites

- .NET 8 SDK
- Docker Desktop (for PostgreSQL)
- Optional: Visual Studio 2026 Insiders or VS Code

## Quick start

1. Clone the repo

```bash
git clone https://github.com/vishipayyallore/vs2026-dev-productivity.git
cd vs2026-dev-productivity
```

2. Restore and build

```bash
dotnet restore
dotnet build VS2026DevProductivity.sln
```

3. Run the Aspire Host (which starts the other services)

```bash
cd src/Aspire.Host
dotnet run
```

Default service URLs (local):

- Aspire Dashboard: <https://localhost:15888>
- API Gateway: <https://localhost:5000>
- Minimal API: <https://localhost:5001>
- Blazor Dashboard: <https://localhost:5002>

## Development notes

- New services live in `src/` and should reference `Aspire.ServiceDefaults` when appropriate.
- Use the Aspire Host to run and debug multiple services at once.

## Contributing

1. Fork and create a branch
2. Make changes and open a PR

## License

MIT — see the `LICENSE` file.
