#!/bin/bash

echo "🚀 Setting up fullstack development environment..."

# Install Angular CLI globally
echo "📦 Installing Angular CLI..."
npm install -g @angular/cli@latest

# Install useful global packages
echo "📦 Installing additional global packages..."
npm install -g typescript ts-node nodemon

# Create sample project structure if it doesn't exist
if [ ! -f "package.json" ]; then
    echo "📁 Creating sample project structure..."
    
    # Create basic package.json
    cat > package.json << 'EOF'
{
  "name": "fullstack-template",
  "version": "1.0.0",
  "description": "Fullstack development template with Angular, .NET, and MongoDB",
  "scripts": {
    "ng": "ng",
    "start": "ng serve --host 0.0.0.0 --port 4200",
    "build": "ng build",
    "watch": "ng build --watch --configuration development",
    "test": "ng test",
    "dev": "concurrently \"npm run start\" \"npm run dotnet:watch\"",
    "dotnet:watch": "cd backend && dotnet watch run",
    "dotnet:build": "cd backend && dotnet build",
    "setup:angular": "ng new frontend --routing --style=scss --skip-git",
    "setup:dotnet": "mkdir -p backend && cd backend && dotnet new webapi"
  },
  "devDependencies": {
    "concurrently": "^8.2.2"
  }
}
EOF

    # Install dependencies
    npm install
fi

# Set up MCP configuration for Claude Code
echo "🤖 Setting up MCP integration..."
mkdir -p ~/.config/claude-code

# Copy MCP server configuration
cp /workspace/.devcontainer/mcp-servers.json ~/.config/claude-code/mcp-settings.json

echo "✅ Development environment setup complete!"
echo ""
echo "🎯 Quick start commands:"
echo "  npm run setup:angular  - Create new Angular project"
echo "  npm run setup:dotnet   - Create new .NET Web API"
echo "  npm run dev           - Start both Angular and .NET in watch mode"
echo ""
echo "🔧 Available services:"
echo "  Angular:  http://localhost:4200"
echo "  .NET API: http://localhost:5000"
echo "  MongoDB:  mongodb://devuser:devpassword@localhost:27017/devdb"
echo ""
echo "🤖 Claude Code with MCP is configured and ready!"