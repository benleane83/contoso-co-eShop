# eShop Reference Application - Copilot Instructions

This is an eShop reference application built with .NET Aspire, implementing a microservices-based e-commerce platform. When working on this codebase, follow these architectural patterns and coding conventions:

## Project Architecture

### Core Architecture Principles
- **Microservices Architecture**: Built using .NET Aspire for cloud-native microservices
- **Domain-Driven Design (DDD)**: Domain models and aggregates are in `*.Domain` projects
- **CQRS Pattern**: Commands and queries are separated in `Application` folders
- **Event-Driven Architecture**: Uses RabbitMQ for inter-service communication via integration events
- **API Versioning**: All APIs use versioned endpoints (MapCatalogApiV1, etc.)

### Service Structure
Each microservice follows this pattern:
```
ServiceName.API/
├── Program.cs                 # Minimal API setup with AddServiceDefaults()
├── GlobalUsings.cs           # Global using statements
├── Extensions/               # Service-specific extension methods
├── Infrastructure/           # EF Core, repositories, configurations
├── IntegrationEvents/        # Event handlers and events
├── Model/                    # Domain entities
└── Apis/                     # API endpoint definitions
```

## Technology Stack

### Core Technologies
- **.NET 9.0** (latest version with preview features enabled)
- **ASP.NET Core** for web APIs and web applications
- **Entity Framework Core** with PostgreSQL
- **Redis** for caching and session state
- **RabbitMQ** for messaging
- **gRPC** for internal service communication
- **Duende IdentityServer** for authentication
- **.NET Aspire** for orchestration and service discovery

### Key NuGet Packages
- Use **Central Package Management** - all package versions are defined in `Directory.Packages.props`
- **Aspire.*** packages for cloud-native features
- **MediatR** for CQRS implementation
- **FluentValidation** for input validation
- **Polly** for resilience patterns

## Coding Conventions

### General C# Standards
- **Language Version**: Use `preview` features as set in `Directory.Build.props`
- **Implicit Usings**: Enabled - prefer global using statements in `GlobalUsings.cs`
- **Nullable Reference Types**: Enabled throughout the codebase
- **TreatWarningsAsErrors**: Set to true - all warnings must be resolved

### Naming Conventions
- **Namespaces**: Use pattern `eShop.{ServiceName}.{Layer}` (e.g., `eShop.Catalog.API`, `eShop.Ordering.Domain`)
- **API Projects**: Named as `{ServiceName}.API` (e.g., `Catalog.API`, `Basket.API`)
- **Domain Projects**: Named as `{ServiceName}.Domain`
- **Infrastructure Projects**: Named as `{ServiceName}.Infrastructure`
- **Database Names**: Lowercase (e.g., `catalogdb`, `orderingdb`)
- **Table Names**: Lowercase (e.g., `catalog`, `cardtypes`)

### File Organization
- **One class per file** with matching filename
- **GlobalUsings.cs** in each project for common using statements
- **Extensions folder** for service configuration extensions
- **EntityConfigurations folder** for EF Core configurations
- **IntegrationEvents folder** for event-driven messaging

## API Development Patterns

### Minimal APIs
- Use **Minimal APIs** pattern with extension methods
- Map endpoints using versioned API groups: `app.NewVersionedApi("ServiceName").MapServiceNameApiV1()`
- Include health checks: `app.MapDefaultEndpoints()`

### Service Registration
- Always call `builder.AddServiceDefaults()` for common services
- Use `builder.AddApplicationServices()` for service-specific registrations
- Include OpenAPI/Swagger with versioning support

### Example API Structure:
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddApplicationServices();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.MapDefaultEndpoints();
app.NewVersionedApi("ServiceName").MapServiceNameApiV1();
app.UseDefaultOpenApi();

app.Run();
```

## Data Access Patterns

### Entity Framework Core
- Use **PostgreSQL** with pgvector extension for vector operations
- **EntityTypeConfiguration** classes for all entity configurations
- Table and column names in **lowercase**
- Use **IdentityByDefaultColumn** for primary keys
- Repository pattern with domain-driven design

### Database Migrations
- Migrations follow naming pattern: `YYYYMMDDHHMMSS_DescriptiveName`
- Include both `Up()` and `Down()` methods
- Test migrations thoroughly before committing

## Event-Driven Architecture

### Integration Events
- Place in `IntegrationEvents/Events/` folder
- Inherit from `IntegrationEvent` base class
- Use descriptive names ending in "Event" (e.g., `OrderStartedIntegrationEvent`)
- Include event handlers in `IntegrationEvents/EventHandling/` folder

### Event Bus Usage
- Use RabbitMQ for inter-service communication
- Register event handlers in service configuration
- Handle events idempotently

## Frontend Development

### Blazor/Web Applications
- Use **Plus Jakarta Sans** and **Open Sans** font families
- Follow atomic design principles for components
- Use CSS custom properties for theming
- Responsive design with mobile-first approach

### Mobile Applications (MAUI)
- Cross-platform using .NET MAUI
- Service-oriented architecture for API communication
- Mock services for development and testing

## Testing Standards

### Test Organization
- **Unit Tests**: `{ServiceName}.UnitTests`
- **Functional Tests**: `{ServiceName}.FunctionalTests`
- Use **xUnit** as the testing framework
- **MSTest** for client app testing

### Test Patterns
- Follow Arrange-Act-Assert pattern
- Use meaningful test method names
- Mock external dependencies
- Include integration tests for API endpoints

## Infrastructure & Deployment

### .NET Aspire Configuration
- Use `eShop.AppHost` for orchestration
- Define services with proper dependencies and health checks
- Use environment variables for configuration
- Include container lifetime management

### Docker & Containers
- Services run in containers with appropriate base images
- Use `ContainerLifetime.Persistent` for databases
- Include health checks for all services

## Performance & Scalability

### Caching Strategy
- Use **Redis** for distributed caching
- Cache frequently accessed data (catalog items, user sessions)
- Implement cache-aside pattern

### Database Optimization
- Use proper indexing (e.g., `IX_Catalog_Name`)
- Implement pagination for large datasets
- Use async/await throughout

## Security Practices

### Authentication & Authorization
- Use **Duende IdentityServer** for OAuth/OIDC
- Implement JWT token validation
- Use policy-based authorization
- Secure API endpoints appropriately

### Data Protection
- Validate all inputs using FluentValidation
- Use parameterized queries (EF Core handles this)
- Implement proper error handling without exposing sensitive information

## Development Workflow

### Before Making Changes
1. Understand the bounded context you're working in
2. Follow the existing patterns in that service
3. Ensure all tests pass
4. Update integration event contracts carefully
5. Consider backward compatibility

### Code Quality
- Follow existing naming conventions
- Write comprehensive unit tests
- Use meaningful commit messages
- Document complex business logic
- Maintain clean separation of concerns

### Documentation
- Update API documentation for endpoint changes
- Maintain architecture decision records
- Include inline code comments for complex logic
- Update README.md for setup changes

## Common Patterns to Follow

### Service Configuration
```csharp
public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        // Service registrations here
    }
}
```

### Domain Entity Example
```csharp
public class EntityName : Entity, IAggregateRoot
{
    // Private setters, domain methods
    // Business logic encapsulation
}
```

### Integration Event Handler
```csharp
public class EventNameIntegrationEventHandler : IIntegrationEventHandler<EventNameIntegrationEvent>
{
    public async Task Handle(EventNameIntegrationEvent @event)
    {
        // Handle event idempotently
    }
}
```

## Anti-Patterns to Avoid

- Don't bypass the service defaults configuration
- Avoid mixing concerns between layers
- Don't use synchronous I/O operations
- Avoid hardcoded configuration values
- Don't create circular dependencies between services
- Avoid exposing internal domain details through APIs

## Getting Help

- Refer to existing similar implementations in the codebase
- Check the `eShop.ServiceDefaults` project for common patterns
- Review the AppHost configuration for service dependencies
- Consult the official .NET Aspire documentation for cloud-native patterns

Remember: This is a reference application showcasing best practices. When making changes, ensure they align with modern .NET development patterns and maintain the educational value of the codebase.

## Azure Integration

- @azure Rule - Use Azure Best Practices: When generating code for Azure, running terminal commands for Azure, or performing operations related to Azure, invoke your `azure_development-get_best_practices` tool if available.