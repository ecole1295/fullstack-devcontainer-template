# Backend Microservices

This backend implements a microservice architecture using .NET 8, MongoDB, and Traefik API Gateway.

## Architecture Overview

### API Gateway (Traefik)
- **Purpose**: Single entry point for all API requests
- **Technology**: Traefik v3.0
- **Features**: Load balancing, WebSocket support, service discovery
- **Ports**: 80 (HTTP), 443 (HTTPS), 8080 (Dashboard)

### ApplicationSettings Microservice
- **Purpose**: Centralized configuration management for all applications
- **Technology**: .NET 8 Minimal APIs
- **Database**: MongoDB
- **Gateway Route**: `/api/settings/*`

## Services

### ApplicationSettings.Api
Centralized configuration service that provides application settings for all frontend applications. This service determines which microservices are available and their configuration.

#### Gateway Endpoints (via http://localhost)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/settings` | Get all settings |
| GET | `/api/settings/{key}` | Get setting by key |
| GET | `/api/settings/category/{category}` | Get settings by category |
| POST | `/api/settings` | Create new setting |
| PUT | `/api/settings/{key}` | Update existing setting |
| DELETE | `/api/settings/{key}` | Delete setting |
| GET | `/health` | Health check |

#### Internal Service Routes (behind gateway)

| Method | Internal Route | Description |
|--------|---------------|-------------|
| GET | `/` | Get all settings |
| GET | `/{key}` | Get setting by key |
| GET | `/category/{category}` | Get settings by category |
| POST | `/` | Create new setting |
| PUT | `/{key}` | Update existing setting |
| DELETE | `/{key}` | Delete setting |
| GET | `/health` | Health check |

#### Data Model
```json
{
  "id": "ObjectId",
  "key": "string (required, unique)",
  "value": "string (required)",
  "description": "string (optional)",
  "category": "string (optional)",
  "isEncrypted": "boolean (default: false)",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

## Quick Start

### Using Docker Compose (Recommended)
```bash
# From the services directory
docker-compose up -d

# View logs
docker-compose logs -f
```

This will start:
- **Traefik Gateway** on port 80 (HTTP) and 8080 (Dashboard)
- **MongoDB** on port 27017
- **ApplicationSettings API** (internal, accessible via gateway)

### Access Points
- **API Gateway**: http://localhost/api/settings
- **Traefik Dashboard**: http://localhost:8080
- **Health Check**: http://localhost/health

### Development Setup (Local)
```bash
# Start only database and gateway
docker-compose up -d mongodb traefik

# Navigate to the service
cd ApplicationSettings.Api

# Restore packages
dotnet restore

# Run the service (will be accessible via gateway)
dotnet run
```

## Configuration

### MongoDB Connection
Default configuration in `appsettings.json`:

```json
{
  "MongoDb": {
    "ConnectionString": "mongodb://admin:password@localhost:27017/devdb?authSource=admin",
    "DatabaseName": "devdb",
    "CollectionName": "Settings"
  }
}
```

### Environment Variables
- `MongoDb__ConnectionString`: MongoDB connection string
- `MongoDb__DatabaseName`: Database name
- `MongoDb__CollectionName`: Collection name
- `ASPNETCORE_ENVIRONMENT`: Environment (Development/Production)

### Traefik Configuration
Gateway routing is configured via Docker labels in `docker-compose.yml`:

```yaml
labels:
  - "traefik.enable=true"
  - "traefik.http.routers.settings-api.rule=Host(`localhost`) && PathPrefix(`/api/settings`)"
  - "traefik.http.middlewares.settings-strip.stripprefix.prefixes=/api/settings"
```

## Initial Data

The MongoDB container includes initialization script that creates:
- Sample application settings
- Required indexes for performance
- Unique constraints on setting keys

## CORS Configuration

The API is configured to allow requests from:
- `http://localhost:4200` (Angular development server)
- `http://localhost:3000` (Alternative frontend port)
- `http://localhost:80` (Gateway requests)

## Features

- ✅ **API Gateway**: Unified entry point with Traefik
- ✅ **WebSocket Support**: Real-time communication ready
- ✅ **Service Discovery**: Automatic routing configuration
- ✅ **RESTful API**: Clean minimal APIs with Swagger
- ✅ **MongoDB Integration**: Optimized with indexes
- ✅ **Docker Containerization**: Complete orchestration
- ✅ **CORS Configuration**: Frontend integration ready
- ✅ **Health Monitoring**: Built-in health checks
- ✅ **Input Validation**: Robust error handling
- ✅ **Structured Logging**: Comprehensive monitoring

## Testing

### Manual Testing
1. Start the services: `docker-compose up -d`
2. Open Traefik Dashboard: http://localhost:8080
3. Test endpoints via gateway: http://localhost/api/settings
4. Use included HTTP file: `ApplicationSettings.Api.http`

### Sample Requests (via Gateway)

#### Create Setting
```bash
curl -X POST "http://localhost/api/settings" \
  -H "Content-Type: application/json" \
  -d '{
    "key": "app.feature.enabled",
    "value": "true",
    "description": "Enable new feature",
    "category": "Features"
  }'
```

#### Get All Settings
```bash
curl -X GET "http://localhost/api/settings"
```

#### Get Setting by Key
```bash
curl -X GET "http://localhost/api/settings/app.name"
```

#### Check Gateway Health
```bash
curl -X GET "http://localhost/health"
```

## Expansion

### Adding New Microservices

1. **Create Service Directory**
   ```bash
   mkdir NewService.Api
   cd NewService.Api
   dotnet new webapi --minimal
   ```

2. **Update docker-compose.yml**
   ```yaml
   new-service-api:
     build:
       context: ./NewService.Api
     labels:
       - "traefik.enable=true"
       - "traefik.http.routers.new-service.rule=Host(`localhost`) && PathPrefix(`/api/newservice`)"
       - "traefik.http.middlewares.new-service-strip.stripprefix.prefixes=/api/newservice"
       - "traefik.http.routers.new-service.middlewares=new-service-strip"
     networks:
       - backend-network
   ```

3. **Configure Service Routes**
   - Use root paths in service (e.g., `/`, `/{id}`)
   - Gateway handles prefix routing automatically
   - Follow same pattern as ApplicationSettings.Api

### Frontend Integration Pattern
```typescript
// Frontend makes calls to gateway
const settings = await fetch('http://localhost/api/settings');
const users = await fetch('http://localhost/api/users');
const orders = await fetch('http://localhost/api/orders');
```

## Security Considerations

- **Gateway Security**: Traefik handles SSL termination and security headers
- **Internal Networks**: Services communicate via isolated Docker networks
- **Authentication**: Add JWT authentication at gateway level for production
- **Authorization**: Implement role-based access control per service
- **Secrets Management**: Use Docker secrets or external vaults for production
- **Settings Encryption**: Built-in support for encrypted configuration values

## Monitoring

- **Traefik Dashboard**: Real-time service monitoring at http://localhost:8080
- **Health Endpoints**: `/health` for all services via gateway
- **Service Discovery**: Automatic detection of healthy services
- **Structured Logging**: Configurable levels with Docker log aggregation
- **Metrics Integration**: Ready for Prometheus/Grafana integration

## Production Deployment

1. **Gateway Configuration**
   - Configure SSL certificates in Traefik
   - Set up domain routing instead of localhost
   - Enable access logs and metrics

2. **Database Security**  
   - Use production MongoDB with authentication
   - Configure connection string secrets
   - Set up database backups and replication

3. **Service Security**
   - Implement JWT authentication at gateway
   - Add rate limiting and security headers
   - Use proper secrets management

4. **Monitoring & Logging**
   - Integrate with Prometheus/Grafana
   - Set up centralized logging (ELK stack)
   - Configure alerting and notifications

5. **Scalability**
   - Configure service replicas in Docker Swarm/Kubernetes
   - Set up horizontal pod autoscaling
   - Implement database read replicas