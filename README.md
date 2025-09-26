# vs2026-dev-productivity

This repository showcases **.NET Aspire + Visual Studio 2026 Insiders features** to maximize developer productivity.

## 🚀 .NET Aspire Starter Project

A comprehensive .NET Aspire application demonstrating modern cloud-native development patterns with:

- **Orchestrated Services**: Multiple microservices managed by Aspire Host
- **API Gateway**: YARP reverse proxy for routing
- **Real-time Dashboard**: Blazor Server app for monitoring
- **Database Integration**: PostgreSQL with Entity Framework Core
- **Observability**: Built-in logging, metrics, and health checks
- **Service Discovery**: Automatic service registration and discovery

## 📁 Project Structure

```
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

### 2. Install .NET Aspire Workload

```bash
dotnet workload install aspire
```

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Run the Application

```bash
cd src/Aspire.Host
dotnet run
```

This will:
- Start the **Aspire Dashboard** at `https://localhost:15888`
- Launch **PostgreSQL** container with pgAdmin
- Start **MinimalApi** service
- Start **ApiGateway** service  
- Start **BlazorApp** dashboard

### 5. Access the Applications

| Service | URL | Description |
|---------|-----|-------------|
| **Aspire Dashboard** | https://localhost:15888 | Main orchestration dashboard |
| **API Gateway** | https://localhost:5000 | YARP reverse proxy |
| **Blazor Dashboard** | https://localhost:5002 | Product management UI |
| **Minimal API** | https://localhost:5001 | Direct API access |
| **pgAdmin** | https://localhost:5433 | PostgreSQL management |

## 📋 Features Implemented

### ✅ Product Management API
- **GET** `/api/products` - List products with pagination
- **GET** `/api/products/{id}` - Get product by ID
- **POST** `/api/products` - Create new product
- **GET** `/api/weather` - Weather forecast example

### ✅ Blazor Dashboard
- Product listing with pagination
- Responsive Bootstrap UI
- Real-time data from API Gateway
- Service health monitoring

### ✅ Infrastructure
- **PostgreSQL** database with Entity Framework Core
- **YARP** API Gateway for request routing
- **Service Discovery** for automatic service registration
- **Health Checks** for all services
- **OpenTelemetry** for distributed tracing and metrics
- **Structured Logging** with Serilog integration

## 🗄️ Database

The application uses **PostgreSQL** with Entity Framework Core:

- **Product** entity with seeded sample data
- Automatic migrations on application startup
- Connection string configured via Aspire service discovery

### Sample Data
```json
[
  {
    "id": 1,
    "name": "Sample Product 1",
    "description": "A sample product for testing",
    "price": 29.99,
    "stock": 100
  },
  {
    "id": 2,
    "name": "Sample Product 2", 
    "description": "Another sample product",
    "price": 49.99,
    "stock": 50
  }
]
```

## 🔧 Development

### Adding New Services

1. Create a new project in the `src/` directory
2. Add reference to `Aspire.ServiceDefaults`
3. Configure service in `Aspire.Host/Program.cs`:

```csharp
var newService = builder.AddProject<Projects.NewService>("new-service")
    .WithReference(database);
```

### Debugging

- Use **Visual Studio 2026 Insiders** for the best debugging experience
- Set multiple startup projects or use the Aspire Host
- View logs and metrics in the Aspire Dashboard

### Testing API Endpoints

Use the built-in Swagger UI at each service endpoint or test directly:

```bash
# Get all products
curl https://localhost:5001/api/products

# Create a product
curl -X POST https://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -d '{"name":"Test Product","description":"Created via API","price":19.99,"stock":25}'
```

## 📊 Observability

The application includes comprehensive observability:

- **Distributed Tracing**: Track requests across services
- **Metrics**: Performance counters and custom metrics
- **Health Checks**: Service availability monitoring
- **Structured Logging**: Centralized log aggregation

Access the **Aspire Dashboard** to view:
- Service topology
- Real-time metrics
- Distributed traces
- Log aggregation
- Resource utilization

## 🔒 Configuration

Environment-specific settings can be configured via:

- `appsettings.json` files in each project
- Environment variables
- Azure Key Vault (in production)
- Aspire configuration providers

## 🚢 Deployment

The application is designed for containerized deployment:

```bash
# Build container images
dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer

# Deploy to Kubernetes (example)
kubectl apply -f k8s/
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit changes: `git commit -m 'Add amazing feature'`
4. Push to branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

## 📚 Learn More

- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [YARP Documentation](https://microsoft.github.io/reverse-proxy/)
- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Built with ❤️ using .NET Aspire and Visual Studio 2026 Insiders**
