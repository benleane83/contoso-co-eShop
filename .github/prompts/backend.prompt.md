# Prompt: Generate a Minimal API Endpoint for eShop Microservice

You are working in the `{ServiceName}.API` project of a .NET Aspire-based microservices e-commerce solution.  
**Follow these conventions:**
- .NET 9 minimal APIs with versioned endpoints (`app.NewVersionedApi("{ServiceName}").Map{ServiceName}ApiV1()`).
- Register services using `builder.AddServiceDefaults()` and `builder.AddApplicationServices()`.
- Use MediatR for CQRS (commands/queries in `Application` folder).
- Validate inputs with FluentValidation.
- Use PostgreSQL via EF Core (entities in `Model/`, configurations in `Infrastructure/EntityConfigurations/`).
- Integration events in `IntegrationEvents/Events/`.
- Use global usings and implicit usings.
- Table and column names are lowercase.
- All code must be nullable-enabled and treat warnings as errors.
- One class per file, matching filename.
- Follow eShop naming conventions and folder structure.

**Task:**  
Add a new endpoint to `{Describe the business task here, e.g., "retrieve a paginated list of items filtered by category"}`.

- **Endpoint:** `{HTTP_METHOD} /api/v1/{resource}/{action}`
- **Query/Route Parameters:** `{List parameters and types here}`
- **Returns:** `{Describe the response DTO and fields}`
- Use async/await and MediatR handler.
- Validate inputs with FluentValidation.
- Follow the existing folder structure and naming conventions.

**Example Usage:**
```
{HTTP_METHOD} /api/v1/{resource}/{action}?{parameters}
```

**Output:**  
- Minimal API endpoint definition (extension method in `Apis/` folder)
- MediatR command/query and handler (in `Application/Commands/` or `Application/Queries/`)
- FluentValidation validator (in `Application/Commands/Validators/` or `Application/Queries/Validators/`)
- DTO for request/response (in `Application/Commands/` or `Application/Queries/`)

**Instructions:**  
- Use DDD, CQRS, and event-driven patterns as in the eShop reference.
- Use lowercase for table/column names.
- Ensure code is nullable-enabled and warnings are treated as errors.
- Write one class per file.
- Use global usings.
- Follow eShop folder and naming conventions.