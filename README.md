# RoboRally Backend

A .NET 9 backend API for the RoboRally game built using Clean Architecture principles with FastEndpoints and Entity Framework Core.

## 🏗️ Project Structure

The project follows Clean Architecture patterns with clear separation of concerns across different layers:

```
Roborally-backend/
├── src/                              # Source code
│   ├── core/                         # Core business logic (Domain & Application layers)
│   │   ├── Roborally.core.domain/    # Domain entities, value objects, and business rules
│   │   └── Roborally.core.application/ # Application services, handlers, and contracts
│   ├── infrastructure/               # Infrastructure layer
│   │   └── Roborally.infrastructure.persistence/ # Data access, Entity Framework, repositories
│   └── Roborally.webapi/            # Web API layer (Presentation)
│       ├── RestEndpoints/           # FastEndpoints API endpoints
│       ├── Properties/              # Launch settings
│       └── scripts/                 # Build and migration scripts
├── tests/                           # Test projects
│   ├── Roborally.unitTests/         # Unit tests
│   └── Roborally.integrationTests/  # Integration tests
├── docs/                           # Documentation
│   └── Documentations/             # Project documentation
├── docker-compose.yml              # Docker container orchestration
└── Roborally-backend.sln           # Visual Studio solution file
```

## 🔧 Technology Stack

- **Framework**: .NET 9
- **Web Framework**: ASP.NET Core with FastEndpoints
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Containerization**: Docker & Docker Compose
- **API Documentation**: Swagger/OpenAPI

## 📋 Architecture Overview

### Core Layer
- **Domain**: Contains business entities, domain services, and business rules
- **Application**: Contains application services, use case handlers, and data transfer objects

### Infrastructure Layer
- **Persistence**: Data access implementation using Entity Framework Core with PostgreSQL

### Presentation Layer
- **WebAPI**: RESTful API endpoints using FastEndpoints framework

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- Docker and Docker Compose
- PostgreSQL (or use the provided Docker setup)

### Running with Docker
```bash
docker-compose up
```

This will start:
- The RoboRally API on port 5000
- PostgreSQL database on default port

### Running Locally
1. Ensure PostgreSQL is running
2. Update connection string in `appsettings.json`
3. Run database migrations
4. Start the application:
```bash
cd src/Roborally.webapi
dotnet run
```

## 🗄️ Database

The project uses Entity Framework Core with PostgreSQL. Database migrations are managed through EF Core tools.

## 📚 API Documentation

The API uses Swagger for documentation. Once running, visit:
- Development: `https://localhost:5001/swagger` (or your configured port)

## 🧪 Testing

The project includes both unit tests and integration tests:

- **Unit Tests**: Located in `tests/Roborally.unitTests/`
- **Integration Tests**: Located in `tests/Roborally.integrationTests/`

Run tests using:
```bash
dotnet test
```

## 🔧 Development

### Project Dependencies
- **Core Domain**: No external dependencies (pure business logic)
- **Core Application**: Depends on Domain layer
- **Infrastructure**: Depends on Application and Domain layers
- **WebAPI**: Depends on Application and Infrastructure layers

### Key Features
- Clean Architecture implementation
- FastEndpoints for high-performance APIs
- Entity Framework Core for data access
- Docker support for easy deployment
- Comprehensive testing setup

## 📝 Contributing

1. Follow Clean Architecture principles
2. Ensure proper separation of concerns
3. Write tests for new features
4. Update documentation as needed

## 🚢 Deployment

The project includes Docker support for easy deployment. Use the provided `docker-compose.yml` for local development or adapt it for production environments.
