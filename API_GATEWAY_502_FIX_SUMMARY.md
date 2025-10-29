# API Gateway 502 Error Fix - Summary

## Problem Analysis

The 502 Bad Gateway error was occurring when calling `/api/weather` from the API Gateway due to several configuration issues:

1. **Protocol Mismatch**: Services were configured with HTTPS but running on HTTP
2. **Service Discovery**: Missing proper service discovery configuration
3. **Service Name**: Incorrect service addressing

## Root Causes Identified

### 1. **API Gateway Configuration (appsettings.json)**

- **Issue**: Destination address was `"https://aspireapp-minimalapi/"`
- **Problem**: Using HTTPS protocol and trailing slash
- **Solution**: Changed to `"http://aspireapp-minimalapi"`

### 2. **BlazorWeb HTTP Client Configuration**

- **Issue**: API Gateway URL was `"https://aspireapp-apigateway"`
- **Problem**: Using HTTPS for internal service communication
- **Solution**: Changed to `"http://aspireapp-apigateway"`

### 3. **Service Discovery in API Gateway**

- **Issue**: Missing service discovery configuration
- **Problem**: YARP couldn't resolve service addresses properly
- **Solution**: Added `builder.Services.AddServiceDiscovery();`

## Changes Made

### 1. **src/AspireApp.ApiGateway/appsettings.json**

```json
"Clusters": {
  "api-cluster": {
    "Destinations": {
      "api-destination": {
        "Address": "http://aspireapp-minimalapi"  // Changed from https://aspireapp-minimalapi/
  }
    }
  }
}
```

### 2. **src/AspireApp.ApiGateway/Program.cs**

```csharp
// Add YARP reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add service discovery for YARP
builder.Services.AddServiceDiscovery();  // Added this line
```

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
