# API Gateway 502 Error Fix - Summary

## Problem Analysis

The 502 Bad Gateway error was occurring when calling `/api/weather` from the API Gateway due to several configuration issues:

1. **Protocol Mismatch**: Services were configured with HTTPS but running on HTTP
2. **Service Discovery**: Missing proper service discovery configuration
3. **Service Name**: Incorrect service addressing

## Root Causes Identified

### Primary Issue: Missing Service Discovery Integration

**The Core Problem**: The API Gateway's YARP reverse proxy was not configured to use Aspire's service discovery mechanism. Without `.AddServiceDiscoveryDestinationResolver()`, YARP could not resolve service names to actual endpoints.

### Secondary Issues (Now Resolved by Service Discovery)

1. **API Gateway Configuration (appsettings.json)**
   - Service name `aspireapp-minimalapi` needs service discovery to resolve to actual endpoint
   - Service discovery handles protocol and address resolution automatically

2. **BlazorWeb HTTP Client Configuration**
   - Internal service communication through API Gateway
   - Aspire manages the actual HTTP/HTTPS protocols

3. **Service Discovery in API Gateway**
   - **Critical**: Must use `.AddServiceDiscoveryDestinationResolver()` for YARP
   - This enables dynamic endpoint resolution for all downstream services

## Changes Made

### 1. **src/AspireApp.ApiGateway/appsettings.json**

**Note**: The destination address uses `https://aspireapp-minimalapi` which is the service name. With `.AddServiceDiscoveryDestinationResolver()` in place, YARP will automatically resolve this to the actual HTTP endpoint managed by Aspire's service discovery. The protocol in the configuration can be HTTPS as the service discovery resolver will handle the actual endpoint resolution.

```json
"Clusters": {
  "api-cluster": {
    "Destinations": {
      "api-destination": {
        "Address": "https://aspireapp-minimalapi"  // Service name - resolved by service discovery
      }
    }
  }
}
```

### 2. **src/AspireApp.ApiGateway/Program.cs**

**Critical Fix**: Added `.AddServiceDiscoveryDestinationResolver()` to enable YARP to resolve service endpoints through Aspire's service discovery.

```csharp
// Configure YARP reverse proxy with service discovery
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();  // This is the key fix!
```

**Why This Works**:

- `.AddServiceDiscoveryDestinationResolver()` integrates YARP with .NET Aspire's service discovery
- Allows YARP to dynamically resolve service names (like `aspireapp-minimalapi`) to actual running endpoints
- Without this, YARP treats the destination address as a literal URL and cannot resolve the service name

### 3. **src/AspireApp.BlazorWeb/Program.cs**

```csharp
builder.Services.AddHttpClient("ApiGateway", client =>
{
    client.BaseAddress = new Uri("http://aspireapp-apigateway");  // Changed from https://
    client.Timeout = TimeSpan.FromSeconds(30);
});
```

## Technical Explanation

### Service Discovery in .NET Aspire

- Aspire uses service discovery to resolve service names to actual endpoints
- Services communicate internally using HTTP, not HTTPS
- Service names match those defined in the AppHost configuration

### YARP Configuration

- YARP (Yet Another Reverse Proxy) routes requests from API Gateway to backend services
- Service discovery integration allows YARP to dynamically resolve service endpoints
- The `http://aspireapp-minimalapi` address gets resolved to the actual running instance

### AppHost Service Registration

From `AppHost.cs`:

```csharp
var api = builder.AddProject<Projects.AspireApp_MinimalApi>("aspireapp-minimalapi")
var gateway = builder.AddProject<Projects.AspireApp_ApiGateway>("aspireapp-apigateway")
```

## Expected Behavior After Fix

1. **BlazorWeb** ? HTTP request to `http://aspireapp-apigateway/api/weather`
2. **API Gateway** ? Routes to `http://aspireapp-minimalapi/api/weather`
3. **MinimalApi** ? Returns weather forecast data
4. **Response** ? Flows back through the chain

## Build Status

? **Build Successful** - All changes compile without errors

## Testing Recommendations

1. **Restart the Aspire Host** to ensure all services pick up new configurations
2. **Check Aspire Dashboard** to verify all services are running and healthy
3. **Test the endpoint** by navigating to the Weather page in the Blazor app
4. **Monitor logs** for any remaining connectivity issues

The 502 error should now be resolved, and the weather forecast should load properly through the API Gateway.
