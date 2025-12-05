# API Documentation

## Base URLs

| Environment | Base URL |
|-------------|----------|
| Local Development | `https://localhost:5000` (via API Gateway) |
| Direct API Access | `https://localhost:5001` (MinimalApi) |

## Authentication

Currently, no authentication is implemented. This is a starter template.

## Products API

### Get All Products

**GET** `/api/products`

Query Parameters:

**Response:**

```json
{
  "products": [
    {
      "id": 1,
      "name": "Sample Product 1",
      "description": "A sample product for testing",
      "price": 29.99,
      "stock": 100,
      "createdAt": "2024-01-01T10:00:00Z",
      "updatedAt": null
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 2,
  "totalPages": 1
}
```

### Get Product by ID

**GET** `/api/products/{id}`

**Response:**

```json
{
  "id": 1,
  "name": "Sample Product 1",
  "description": "A sample product for testing",
  "price": 29.99,
  "stock": 100,
  "createdAt": "2024-01-01T10:00:00Z",
  "updatedAt": null
}
```

**Error Response (404):**

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "Product with ID 999 not found"
}
```

### Create Product

**POST** `/api/products`

**Request Body:**

```json
{
  "name": "New Product",
  "description": "Product description",
  "price": 19.99,
  "stock": 50
}
```

**Response (201 Created):**

```json
{
  "id": 3,
  "name": "New Product", 
  "description": "Product description",
  "price": 19.99,
  "stock": 50,
  "createdAt": "2024-01-01T10:00:00Z",
  "updatedAt": null
}
```

## Weather API (Example)

### Get Weather Forecast

**GET** `/api/weather`

**Response:**

```json
[
  {
    "date": "2024-01-02",
    "temperatureC": 15,
    "temperatureF": 59,
    "summary": "Cool"
  },
  {
    "date": "2024-01-03",
    "temperatureC": 22,
    "temperatureF": 71,
    "summary": "Mild"
  }
]
```

## Stock Prices API

### Get Stock Prices

**GET** `/api/stocks?symbols={comma-separated-symbols}`

Retrieve current stock prices for the specified symbols.

**Query Parameters:**
- `symbols` (required): Comma-separated list of stock symbols (e.g., "AAPL,MSFT,GOOG")

**Response (200 OK):**

```json
[
  {
    "symbol": "AAPL",
    "currentPrice": 177.25,
    "dailyChange": 2.50,
    "dailyChangePercent": 1.43,
    "lastUpdated": "2024-12-05T08:00:00Z"
  },
  {
    "symbol": "MSFT",
    "currentPrice": 375.25,
    "dailyChange": -3.00,
    "dailyChangePercent": -0.79,
    "lastUpdated": "2024-12-05T08:00:00Z"
  }
]
```

**Error Response (400 Bad Request):**

```json
{
  "error": "At least one stock symbol must be provided"
}
```

**Error Response (503 Service Unavailable):**

```json
{
  "type": "about:blank",
  "title": "Stock data temporarily unavailable",
  "status": 503,
  "detail": "Service unavailable"
}
```

## System Endpoints

### Health Check

**GET** `/health`

**Response:**

```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0234567",
  "entries": {
    "self": {
      "status": "Healthy"
    }
  }
}
```

### API Status

**GET** `/`

**Response:**

```json
{
  "service": "Aspire.MinimalApi",
  "status": "Running",
  "timestamp": "2024-01-01T10:00:00Z"
}
```

## Error Handling

All APIs use standard HTTP status codes:

Error responses follow RFC 7807 Problem Details format:

```json
{
  "type": "about:blank",
  "title": "Bad Request",
  "status": 400,
  "detail": "The request body is invalid."
}
```

## Rate Limiting

Currently no rate limiting is implemented. Consider adding:

## API Explorer (Scalar)

Interactive API documentation is available via the integrated Scalar UI at:

- **Development**: `https://localhost:5001/scalar/v1` (when Scalar is enabled in configuration)

## Example Requests

### cURL Examples

```bash
# Get all products
curl -X GET "https://localhost:5001/api/products" -H "accept: application/json"

# Get product by ID
curl -X GET "https://localhost:5001/api/products/1" -H "accept: application/json"

# Create new product
curl -X POST "https://localhost:5001/api/products" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Product",
    "description": "Created via cURL",
    "price": 25.99,
    "stock": 75
  }'

# Get stock prices
curl -X GET "https://localhost:5001/api/stocks?symbols=AAPL,MSFT,GOOG" \
  -H "accept: application/json"
```

### PowerShell Examples

```powershell
# Get all products
Invoke-RestMethod -Uri "https://localhost:5001/api/products" -Method Get

# Create new product
$body = @{
    name = "PowerShell Product"
    description = "Created via PowerShell"
    price = 15.99
    stock = 30
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:5001/api/products" -Method Post -Body $body -ContentType "application/json"

# Get stock prices
Invoke-RestMethod -Uri "https://localhost:5001/api/stocks?symbols=AAPL,MSFT,GOOG" -Method Get
```
