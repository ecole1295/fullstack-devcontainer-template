# Fuse Angular Templates Documentation

## Overview

The Fuse Angular template system provides two main templates for building Angular admin applications:
- **fuse-demo**: Full-featured template with working examples and mock data
- **fuse-starter**: Clean starter template for new applications without mock data

Both templates are built with Angular 19, Angular Material, and TailwindCSS.

## Template Structure

### Core Features
- Angular 19.0.5
- Angular Material 19.0.4
- TailwindCSS 3.4.17
- Internationalization (i18n) with Transloco
- Multiple layout options
- Authentication system
- Mock API system
- Component library (@fuse)

## Fuse Demo Template

### Purpose
The demo template showcases all available components and features with working examples and mock data. Use this to:
- Understand component implementations
- See real examples of features in action
- Learn patterns and best practices
- Reference implementation details

### Key Modules

#### Admin Apps
- **Academy**: Course management system with details and listing views
- **Chat**: Real-time messaging interface with contacts and conversations
- **Contacts**: Contact management with detailed views
- **E-commerce**: Inventory management system
- **File Manager**: File browser with list and detail views
- **Help Center**: FAQ, guides, and support system
- **Mailbox**: Email client with compose, settings, and sidebar
- **Notes**: Note-taking app with labels and organization
- **Scrumboard**: Kanban-style project management
- **Tasks**: Task management with detailed views

#### Dashboards
- **Analytics**: Data visualization dashboard
- **Crypto**: Cryptocurrency tracking dashboard
- **Finance**: Financial metrics dashboard
- **Project**: Project management overview

#### Pages
- **Authentication**: Multiple auth layouts (classic, modern, fullscreen, split-screen)
- **Coming Soon**: Various coming soon page layouts
- **Error Pages**: 404, 500 error pages
- **Invoice**: Printable invoice templates
- **Maintenance**: Maintenance mode pages
- **Pricing**: Different pricing page layouts
- **Profile**: User profile pages
- **Settings**: User settings with multiple tabs

#### UI Components
- **Advanced Search**: Search functionality
- **Animations**: Fuse animation examples
- **Cards**: Various card layouts
- **Colors**: Color system documentation
- **Confirmation Dialog**: Modal confirmations
- **Data Table**: Table components
- **Forms**: Form layouts, fields, and wizards
- **Fuse Components**: Custom Fuse component library
- **Icons**: Icon system (Feather, Heroicons, Material)
- **Material Components**: Angular Material examples
- **Page Layouts**: Layout system examples
- **TailwindCSS**: Utility class examples
- **Typography**: Text styling system

### Directory Structure
```
fuse-demo/
├── src/
│   ├── @fuse/                    # Core Fuse framework
│   │   ├── animations/           # Animation utilities
│   │   ├── components/           # Reusable components
│   │   ├── directives/           # Custom directives
│   │   ├── lib/                  # Core libraries (mock-api)
│   │   ├── pipes/                # Custom pipes
│   │   ├── services/             # Core services
│   │   ├── styles/               # Styling system
│   │   └── validators/           # Form validators
│   ├── app/
│   │   ├── core/                 # Core app services
│   │   ├── layout/               # Layout components
│   │   ├── mock-api/             # Mock data services
│   │   └── modules/              # Feature modules
│   └── public/                   # Static assets
```

## Fuse Starter Template

### Purpose
The starter template provides a clean foundation for new applications without demo content. Use this to:
- Start new projects from scratch
- Avoid removing demo content
- Begin with essential features only
- Build custom applications

### Key Modules

#### Included Features
- **Authentication**: Complete auth system (sign-in, sign-up, forgot password, etc.)
- **Landing Page**: Basic home page
- **Example Module**: Single example component for reference

#### Minimal Structure
- Core Fuse framework (@fuse)
- Essential layout components
- Authentication system
- Basic routing structure
- Clean starting point

### Directory Structure
```
fuse-starter/
├── src/
│   ├── @fuse/                    # Core Fuse framework (identical to demo)
│   ├── app/
│   │   ├── core/                 # Core app services
│   │   ├── layout/               # Layout components
│   │   └── modules/
│   │       ├── admin/
│   │       │   └── example/      # Single example component
│   │       ├── auth/             # Authentication modules
│   │       └── landing/          # Landing page
│   └── public/                   # Static assets
```

## Core Fuse Framework (@fuse)

### Components
- **Alert**: Notification alerts
- **Card**: Content cards
- **Drawer**: Side panels
- **Fullscreen**: Fullscreen toggle
- **Highlight**: Code highlighting
- **Loading Bar**: Progress indicators
- **Masonry**: Masonry layouts
- **Navigation**: Navigation components

### Services
- **Config**: Application configuration
- **Confirmation**: Modal confirmations
- **Loading**: Loading states
- **Media Watcher**: Responsive breakpoints
- **Platform**: Platform detection
- **Splash Screen**: App initialization
- **Utils**: Utility functions

### Layout System

Both templates include multiple layout options:

#### Vertical Layouts
- Classic
- Classy
- Compact
- Dense
- Futuristic
- Thin

#### Horizontal Layouts
- Centered
- Enterprise
- Material
- Modern

#### Special Layouts
- Empty (for auth pages, etc.)

## Getting Started

### Using Demo Template
**⚠️ DO NOT MODIFY - Reference Only**
1. Use for learning and reference purposes only
2. Explore modules to understand implementations
3. Study component patterns and architecture
4. Reference mock data structures
5. Never modify demo files - they serve as documentation

### Using Starter Template
**✅ Copy for New Projects**
1. Copy the entire fuse-starter directory to create new applications
2. Rename the copied directory to your project name
3. Update package.json with your project details
4. Remove the example module when ready
5. Build your features using Fuse components and patterns from demo
6. Follow authentication patterns already established

### Development Commands
```bash
# Navigate to app directory first
cd workspace/apps/fuse-demo  # or fuse-starter

# Install dependencies
npm install

# Start development server
npm start

# Build for production
npm run build

# Run tests
npm test

# Watch build
npm run watch
```

## Backend Integration

### API Gateway Integration
Both Fuse applications are configured to work with the backend microservices through the API Gateway:

```typescript
// app/core/config/app.config.ts
export const appConfig: ApplicationConfig = {
  providers: [
    // HTTP client configured for gateway
    provideHttpClient(withInterceptors([
      // Add gateway base URL interceptor
      (req, next) => {
        const baseUrl = 'http://localhost';
        const apiReq = req.clone({
          url: `${baseUrl}${req.url}`
        });
        return next(apiReq);
      }
    ]))
  ]
};
```

### Configuration Service
Load application settings from the ApplicationSettings microservice:

```typescript
// app/core/config/config.service.ts
@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  private config$ = new BehaviorSubject<AppConfig | null>(null);

  async loadConfig(): Promise<void> {
    try {
      // Load settings from gateway
      const settings = await this.http.get<ApplicationSetting[]>('/api/settings')
        .toPromise();
      
      const config = this.transformSettings(settings);
      this.config$.next(config);
      
      // Apply dynamic configuration
      this.applyThemeConfiguration(config);
      this.setupNavigation(config);
      
    } catch (error) {
      console.error('Failed to load app configuration:', error);
    }
  }

  private transformSettings(settings: ApplicationSetting[]): AppConfig {
    return {
      appName: this.getSetting(settings, 'app.name'),
      theme: {
        primaryColor: this.getSetting(settings, 'app.theme.primary_color'),
        scheme: this.getSetting(settings, 'app.theme.scheme') || 'auto'
      },
      features: {
        userManagement: this.getSetting(settings, 'features.user_management') === 'true',
        analytics: this.getSetting(settings, 'features.analytics') === 'true'
      }
    };
  }
}
```

### Dynamic Navigation
Configure navigation based on available microservices:

```typescript
// app/layout/common/navigation/navigation.service.ts
export class NavigationService {
  buildNavigation(config: AppConfig): FuseNavigationItem[] {
    const navigation: FuseNavigationItem[] = [
      {
        id: 'dashboards',
        title: 'Dashboards',
        subtitle: 'Unique dashboard designs',
        type: 'group',
        icon: 'heroicons_outline:home',
        children: [
          {
            id: 'dashboards.analytics',
            title: 'Analytics',
            type: 'basic',
            icon: 'heroicons_outline:chart-pie',
            link: '/dashboards/analytics'
          }
        ]
      }
    ];

    // Add conditional navigation based on microservice availability
    if (config.features.userManagement) {
      navigation.push({
        id: 'users',
        title: 'User Management',
        type: 'basic',
        icon: 'heroicons_outline:users',
        link: '/users'
      });
    }

    return navigation;
  }
}
```

## Key Dependencies

### Core Angular
- @angular/core: 19.0.5
- @angular/material: 19.0.4
- @angular/cdk: 19.0.4

### UI/UX
- tailwindcss: 3.4.17
- @angular/animations: 19.0.5
- perfect-scrollbar: 1.5.6

### Features
- @jsverse/transloco: 7.5.1 (i18n)
- apexcharts: 4.3.0 (charts)
- ngx-quill: 27.0.0 (rich text)
- highlight.js: 11.11.1 (code highlighting)

### Utilities
- lodash-es: 4.17.21
- luxon: 3.5.0 (date handling)
- crypto-js: 4.2.0

## Best Practices

1. **Template Usage**:
   - **Demo**: Reference only - never modify these files
   - **Starter**: Copy entire directory for each new project
   - Keep demo intact for ongoing reference

2. **Project Setup**:
   - Copy fuse-starter to new directory for each project
   - Update package.json name, description, and version
   - Initialize new git repository for the copied project
   - Remove example module when you're ready to build features

3. **Component Development**:
   - Study demo implementations before building features
   - Follow Fuse component patterns from examples
   - Use provided services and utilities
   - Leverage layout system options

4. **Styling**:
   - Use TailwindCSS utilities
   - Follow Fuse theming system
   - Maintain responsive design
   - Reference demo for styling patterns

5. **Architecture**:
   - Organize by feature modules (follow demo structure)
   - Use lazy loading for routes
   - Implement proper service abstraction
   - Follow authentication patterns from starter template

## Authentication System

Both templates include a complete authentication system with:
- Sign in/Sign up
- Forgot password
- Reset password
- Session unlock
- Confirmation required
- Sign out

Multiple UI layouts available for each auth flow.

## Mock API System (Demo Only)

The demo template includes a comprehensive mock API system for:
- Simulating backend responses
- Development without backend dependency
- Testing and prototyping
- Data structure examples

## Quick Start Workflow

1. **Study the Demo** (fuse-demo):
   - Explore existing components and patterns
   - Understand the architecture and structure
   - Reference implementation examples
   - **Never modify demo files**

2. **Create New Project**:
   ```bash
   # Copy starter template
   cp -r fuse-starter my-new-project
   cd my-new-project
   
   # Update project details
   # Edit package.json name, description, version
   
   # Install dependencies
   npm install
   
   # Start development
   npm start
   ```

3. **Development Process**:
   - Reference demo for implementation patterns
   - Build features using Fuse components
   - Follow established authentication structure
   - Use demo mock data patterns as reference

## Conclusion

The Fuse templates provide a robust foundation for Angular admin applications. Use the **demo template for reference only** - it serves as living documentation. **Copy the starter template** for each new project to ensure clean separation and preserve examples for future reference. Both leverage the powerful Fuse framework with Angular Material and TailwindCSS for professional, responsive applications.