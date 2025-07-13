# ApplicationSettings Microservice

Centralized configuration management service that provides application settings for frontend applications. This service acts as the configuration authority, determining which microservices are available and their configuration.

## Purpose

The ApplicationSettings service serves as a configuration hub that:
- Stores application-wide configuration values
- Provides initial configuration data to frontend applications on startup
- Manages feature flags and environment-specific settings
- Maintains microservice discovery information

## Architecture

- **Framework**: .NET 8 Minimal APIs
- **Database**: MongoDB with optimized indexes
- **Access**: Via Traefik API Gateway at `/api/settings`
- **Pattern**: Domain-driven design with repository pattern

## API Endpoints

All endpoints are accessible via the API Gateway at `http://localhost/api/settings`

### Core Operations

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Get all settings |
| GET | `/{key}` | Get specific setting by key |
| GET | `/category/{category}` | Get settings by category |
| POST | `/` | Create new setting |
| PUT | `/{key}` | Update existing setting |
| DELETE | `/{key}` | Delete setting |

### Health & Monitoring

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/health` | Health check endpoint |

## Data Model

```csharp
public class ApplicationSetting
{
    public string? Id { get; set; }                    // MongoDB ObjectId
    public required string Key { get; set; }           // Unique setting key
    public required string Value { get; set; }         // Setting value
    public string? Description { get; set; }           // Human-readable description
    public string? Category { get; set; }              // Grouping category
    public bool IsEncrypted { get; set; } = false;     // Encryption flag
    public DateTime CreatedAt { get; set; }            // Creation timestamp
    public DateTime UpdatedAt { get; set; }            // Last update timestamp
}
```

### Example Settings

```json
[
  {
    "key": "app.name",
    "value": "Fullstack Application",
    "description": "Application display name",
    "category": "General"
  },
  {
    "key": "app.theme.primary_color",
    "value": "#1976d2",
    "description": "Primary UI color",
    "category": "Theme"
  },
  {
    "key": "services.user_api.enabled",
    "value": "true",
    "description": "Enable user management service",
    "category": "Services"
  }
]
```

## Database Configuration

### MongoDB Settings
- **Database**: `devdb`
- **Collection**: `Settings`
- **Connection**: `mongodb://admin:password@mongodb:27017/devdb?authSource=admin`

### Indexes
- **Unique Index**: `key` field for fast lookups
- **Category Index**: `category` field for grouped queries
- **Timestamp Index**: `createdAt` field for chronological sorting

## Usage Examples

### Frontend Integration

```typescript
// Angular service example
@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private readonly baseUrl = 'http://localhost/api/settings';

  async getAppConfig(): Promise<ApplicationConfig> {
    // Get all settings on app initialization
    const settings = await fetch(`${this.baseUrl}`).then(r => r.json());
    
    return {
      appName: settings.find(s => s.key === 'app.name')?.value,
      theme: {
        primaryColor: settings.find(s => s.key === 'app.theme.primary_color')?.value
      },
      services: {
        userApiEnabled: settings.find(s => s.key === 'services.user_api.enabled')?.value === 'true'
      }
    };
  }

  async updateSetting(key: string, value: string): Promise<void> {
    await fetch(`${this.baseUrl}/${key}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ key, value })
    });
  }
}
```

### cURL Examples

```bash
# Get all settings
curl http://localhost/api/settings

# Get specific setting
curl http://localhost/api/settings/app.name

# Get settings by category
curl http://localhost/api/settings/category/Theme

# Create new setting
curl -X POST http://localhost/api/settings \
  -H "Content-Type: application/json" \
  -d '{
    "key": "feature.new_dashboard",
    "value": "false",
    "description": "Enable new dashboard UI",
    "category": "Features"
  }'

# Update setting
curl -X PUT http://localhost/api/settings/feature.new_dashboard \
  -H "Content-Type: application/json" \
  -d '{
    "key": "feature.new_dashboard",
    "value": "true",
    "description": "Enable new dashboard UI",
    "category": "Features"
  }'

# Delete setting
curl -X DELETE http://localhost/api/settings/feature.new_dashboard
```

## Development

### Local Development
```bash
# Start dependencies (MongoDB + Gateway)
docker-compose up -d mongodb traefik

# Run service locally
cd ApplicationSettings.Api
dotnet run

# Service will be available via gateway at http://localhost/api/settings
```

### Testing
```bash
# Run unit tests
dotnet test

# Test via HTTP file
# Open ApplicationSettings.Api.http in VS Code with REST Client extension
```

### Configuration Management

#### Environment Variables
```bash
# Override connection string
export MongoDb__ConnectionString="mongodb://prod-server:27017/proddb"

# Set environment
export ASPNETCORE_ENVIRONMENT=Production
```

#### Docker Override
```yaml
# docker-compose.override.yml
version: '3.8'
services:
  application-settings-api:
    environment:
      - MongoDb__ConnectionString=mongodb://custom-host:27017/customdb
```

## Best Practices

### Configuration Keys
- Use hierarchical keys: `app.theme.primary_color`
- Follow naming convention: lowercase with dots
- Group related settings by category
- Keep keys descriptive but concise

### Frontend Usage
1. Load all settings on application startup
2. Cache settings in application state
3. Subscribe to setting changes for real-time updates
4. Use settings to determine available features/services

### Security
- Mark sensitive settings with `isEncrypted: true`
- Validate setting values before storage
- Implement proper authorization for admin operations
- Use HTTPS in production

## Monitoring

- Health endpoint: `http://localhost/health`
- Traefik dashboard: `http://localhost:8080`
- MongoDB monitoring via connection metrics
- Structured logging for all operations

## Future Enhancements

- **Real-time Updates**: WebSocket notifications for setting changes
- **Encryption**: Automatic encryption/decryption for sensitive values
- **Validation**: Schema validation for setting values
- **Versioning**: Setting change history and rollback capability
- **Import/Export**: Bulk configuration management
- **Environment Profiles**: Different settings per environment