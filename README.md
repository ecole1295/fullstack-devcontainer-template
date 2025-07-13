# Fullstack DevContainer Template

A comprehensive development container template for fullstack web development with Angular, .NET microservices, MongoDB, API Gateway, and Claude Code integration.

## Features

- **Frontend**: Angular applications with TypeScript
- **Backend**: .NET 8 microservices architecture
- **API Gateway**: Traefik for unified API routing and WebSocket support
- **Database**: MongoDB 7.0 with automatic initialization
- **AI Assistant**: Claude Code with MCP integration
- **Extensions**: Pre-configured VS Code extensions for all technologies

## Quick Start

1. **Open in DevContainer**
   ```bash
   code .
   # Command Palette: "Dev Containers: Reopen in Container"
   ```

2. **Start Services**
   ```bash
   # Start all services (gateway, microservices, database)
   cd workspace/services
   docker-compose up -d
   ```

3. **Start Frontend Development**
   ```bash
   # Start Angular development server
   cd workspace/apps/fuse-demo  # or fuse-starter
   npm install
   npm start
   ```

## Services & Ports

| Service | Port | URL |
|---------|------|-----|
| **API Gateway** | 80 | http://localhost |
| Traefik Dashboard | 8080 | http://localhost:8080 |
| Angular Dev Server | 4200 | http://localhost:4200 |
| MongoDB | 27017 | mongodb://admin:password@localhost:27017/devdb |

### API Endpoints (via Gateway)

| Endpoint | Service | Description |
|----------|---------|-------------|
| `/api/settings/*` | ApplicationSettings | Configuration management |
| `/health` | Health Check | Service health monitoring |

## Claude Code Integration

### MCP Servers Available

1. **Filesystem MCP**: Access project files and structure
2. **MongoDB MCP**: Official MongoDB MCP server for database operations

### Example Claude Code Commands

```bash
# Ask Claude about your codebase
claude "Analyze the project structure"

# Database queries through MCP
claude "Show me the sample data in MongoDB"

# Code generation
claude "Create a new Angular component for user management"
```

## MongoDB Setup

Default credentials:
- **Admin**: admin/password
- **Dev User**: devuser/devpassword  
- **Database**: devdb

## Available Scripts

### Services (Backend)
```bash
cd workspace/services

# Start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down

# Rebuild services
docker-compose up --build -d
```

### Frontend (Angular Apps)
```bash
cd workspace/apps/fuse-demo  # or fuse-starter

npm install           # Install dependencies
npm start            # Start dev server (port 4200)
npm run build        # Build for production
npm test             # Run unit tests
npm run lint         # Run linting
```

## Project Structure

```
├── .devcontainer/
│   ├── devcontainer.json        # DevContainer configuration
│   ├── docker-compose.yml       # DevContainer services
│   ├── setup.sh                 # Post-create setup script
│   └── mongo-init/
│       └── init-mongo.js        # DevContainer MongoDB initialization
├── workspace/
│   ├── apps/                    # Frontend applications
│   │   ├── fuse-demo/          # Full-featured Angular app with Fuse theme
│   │   └── fuse-starter/       # Minimal Angular starter with Fuse theme
│   └── services/               # Backend microservices
│       ├── traefik.yml         # API Gateway configuration
│       ├── docker-compose.yml  # Services orchestration
│       ├── init-mongo.js       # Services MongoDB initialization
│       └── ApplicationSettings.Api/  # Configuration microservice
│           ├── Controllers/
│           ├── Models/
│           ├── Services/
│           └── Dockerfile
└── README.md
```

## VS Code Extensions Included

- Angular Language Service
- C# Dev Kit
- MongoDB for VS Code
- Prettier Code Formatter
- ESLint
- Tailwind CSS IntelliSense
- Auto Rename Tag
- Path Intellisense

## Architecture

### API Gateway Pattern
All frontend requests go through the Traefik gateway at `http://localhost`, which routes to appropriate microservices:

- **Unified API**: Single endpoint for all backend services
- **WebSocket Support**: Real-time communication enabled
- **Load Balancing**: Automatic service discovery and routing
- **Health Monitoring**: Built-in health checks and monitoring dashboard

### Microservices
- **ApplicationSettings**: Centralized configuration management
- **Future Services**: Add new microservices with simple Docker labels

## Environment Variables

The container sets up these environment variables:
- `DOTNET_CLI_TELEMETRY_OPTOUT=1`
- `DOTNET_NOLOGO=1`

## Customization

### Adding New Microservices
1. Create service in `workspace/services/`
2. Add to `docker-compose.yml` with Traefik labels
3. Configure routing rules

### Adding New Extensions  
Edit `.devcontainer/devcontainer.json` and add to the `extensions` array.

### Modifying MongoDB Configuration
Edit `workspace/services/init-mongo.js` for services database setup.

## Troubleshooting

### Services Not Starting
```bash
# Check all services status
cd workspace/services
docker-compose ps

# View specific service logs
docker-compose logs application-settings-api
docker-compose logs traefik

# Restart services
docker-compose restart
```

### Gateway Routing Issues
- Check Traefik dashboard at http://localhost:8080
- Verify service labels in docker-compose.yml
- Ensure services are in same network

### MongoDB Connection Issues
```bash
# Check MongoDB status
cd workspace/services
docker-compose logs mongodb

# Test connection
docker exec -it applications-mongodb mongosh --eval "db.adminCommand('ismaster')"
```

### Port Conflicts
- Gateway uses port 80 (can be changed in docker-compose.yml)
- Traefik dashboard uses port 8080
- Angular dev server uses port 4200