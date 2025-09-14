# RoboRally Backend Architecture

## Overview

This project implements a **Clean Architecture** following **Onion/Hexagonal Architecture** principles. The architecture ensures separation of concerns, testability, and maintainability by organizing code into distinct layers with clear dependency rules.

## Architectural Principles

### Dependency Rule
Dependencies point inward - outer layers depend on inner layers, never the reverse:
```
Infrastructure → Application → Domain
     ↑              ↑
  Web API    →  Application
```

### Core Principles
- **Independence**: The core business logic is independent of frameworks, databases, and external concerns
- **Testability**: Business rules can be tested without UI, database, or external dependencies
- **Flexibility**: Easy to change infrastructure components without affecting business logic

## Layer Structure

### 1. Domain Layer (Center)
**Location**: `src/core/Roborally.core.domain/`

The innermost layer containing pure business logic with **no external dependencies**.

**Components**:
- **Entities**: Business objects (e.g., `User.cs`)
- **Interfaces**: Repository contracts (e.g., `IUserRepository.cs`)
- **Base Abstractions**: Core contracts like `IUnitOfWork.cs`

**Key Characteristics**:
- No dependencies on external frameworks
- Contains business rules and domain logic
- Defines contracts for outer layers to implement

```csharp
// Example: Domain Entity
public class User {
    public Guid Id { get; init; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public DateOnly Birthday { get; set; }
}
```

### 2. Application Layer
**Location**: `src/core/Roborally.core.application/`

Contains application-specific business rules and orchestrates domain objects.

**Components**:
- **Commands**: Request objects implementing `ICommand<TResponse>`
- **Command Handlers**: Business logic processors implementing `ICommandHandler<TCommand, TResponse>`
- **Contracts**: Data transfer objects and service interfaces

**Key Patterns**:
- **CQRS (Command Query Responsibility Segregation)**
- **Mediator Pattern** via FastEndpoints command handling
- **Command Handler Pattern**

```csharp
// Example: Command and Handler
public class SignupCommand : ICommand<Guid> {
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required DateOnly Birthday { get; set; }
}

public class SignupCommandHandler : ICommandHandler<SignupCommand, Guid> {
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    // Business logic implementation with transaction management
}
```

### 3. Infrastructure Layer
**Location**: `src/infrastructure/Roborally.infrastructure.persistence/`

Implements interfaces defined in inner layers, handling external concerns.

**Components**:
- **Database Context**: Entity Framework Core `AppDatabaseContext`
- **Repository Implementations**: Concrete data access (e.g., `UserRepository`)
- **Unit of Work Implementation**: Transaction management
- **Entity Configurations**: EF Core mapping configurations

### 4. Presentation Layer (Web API)
**Location**: `src/Roborally.webapi/`

Handles HTTP requests and implements the **REPR Pattern**.

## Key Patterns & Technologies

### REPR Pattern (Request-Endpoint-Response Pattern)
Implemented using **FastEndpoints** for high-performance, minimal APIs.

**Structure**:
```csharp
public class SignupUser : Endpoint<SignUpUserRequest, SignUpUserResponse> {
    public override void Configure() {
        Post("/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SignUpUserRequest req, CancellationToken ct) {
        // Convert request to command
        SignupCommand command = new SignupCommand() {
            Birthday = req.Birthday,
            Password = req.Password,
            Username = req.Username
        };
        
        // Execute command and return response
        Guid userid = await command.ExecuteAsync(ct);
        await SendOkAsync(new SignUpUserResponse { UserId = userid }, ct);
    }
}
```

**Benefits**:
- **Type Safety**: Strong typing for requests and responses
- **Performance**: Minimal overhead compared to traditional controllers
- **Clarity**: Single responsibility per endpoint
- **Validation**: Built-in request validation

### Command Pattern & CQRS
Commands represent write operations in the system:

**Command Flow**:
1. **Endpoint** receives HTTP request
2. **Request** mapped to **Command**
3. **Command** executed via **Command Handler**
4. **Handler** uses **Domain Services** and **Repositories**
5. **Unit of Work** manages transactions
6. **Response** returned to client

### Entity Framework Core & Unit of Work
**Database Technology**: PostgreSQL with Entity Framework Core

**Unit of Work Pattern**:
```csharp
public class UnitOfWork : IUnitOfWork {
    private readonly AppDatabaseContext _context;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
```

**Transaction Management**:
- **Atomic Operations**: All changes saved together or rolled back
- **Consistency**: Ensures data integrity across multiple repository operations
- **Scoped Lifetime**: One unit of work per HTTP request
- **Async Operations**: Non-blocking database operations

**Database Context**:
```csharp
public class AppDatabaseContext : DbContext {
    public required DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDatabaseContext).Assembly);
    }
}
```

### Dependency Injection & Module Installation

**Application Module Registration**:
- Automatic discovery and registration of command handlers
- Generic command registration using reflection
- Scoped lifetime management

**Infrastructure Module Registration**:
- Database context configuration
- Repository implementations
- Unit of Work registration

## Architecture Benefits

### 1. **Testability**
- **Unit Testing**: Domain logic tested in isolation
- **Integration Testing**: Test complete workflows
- **Mocking**: Easy to mock repository interfaces

### 2. **Maintainability**
- **Separation of Concerns**: Each layer has a single responsibility
- **Loose Coupling**: Layers communicate through interfaces
- **Clear Dependencies**: Dependency direction is explicit

### 3. **Flexibility**
- **Database Independence**: Can switch databases without changing business logic
- **Framework Independence**: Core logic independent of web frameworks
- **Scalability**: Easy to scale individual components

### 4. **Performance**
- **FastEndpoints**: High-performance HTTP handling
- **EF Core**: Optimized database operations
- **Async/Await**: Non-blocking operations throughout

## Data Flow Example

### User Signup Flow:
1. **HTTP POST** `/api/users` with `SignUpUserRequest`
2. **SignupUser Endpoint** receives and validates request
3. **SignupCommand** created from request data
4. **SignupCommandHandler** processes the command:
   - Creates new `User` domain entity
   - Calls `IUserRepository.AddAsync()`
   - Calls `IUnitOfWork.SaveChangesAsync()` for transaction
5. **SignUpUserResponse** returned with new user ID

This flow demonstrates the clear separation between presentation concerns (HTTP), application logic (command handling), and infrastructure concerns (database persistence).

## Technology Stack Summary

- **.NET 9**: Modern C# features and performance
- **FastEndpoints**: High-performance web API framework
- **Entity Framework Core**: ORM with PostgreSQL
- **PostgreSQL**: Robust relational database
- **Docker**: Containerization for deployment
- **Clean Architecture**: Maintainable and testable structure
