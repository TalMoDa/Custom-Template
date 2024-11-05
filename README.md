
# TalMoDa.Custom.Template

**A .NET 6 Web API template by Tal Moshe Dayan**  
This template is designed to streamline the creation of a .NET 6 Web API project with commonly used components and configurations, enabling rapid development with best practices for logging, dependency injection, error handling, and more.

---

## Features

- **Entity Framework Core**: Database setup and scaffolding for easy interaction with SQL databases.
- **Repository Pattern**: Base and custom repositories for data access with async methods.
- **CQRS with MediatR**: Implements CQRS for handling queries and commands, with FluentValidation for request validation.
- **Logging with Serilog**: Structured logging using Serilog, with options for logging errors and warnings.
- **Global Error Handling**: Centralized error handling with custom problem details.
- **Swagger Integration**: API documentation is provided through Swagger.
- **Dependency Injection Setup**: Pre-configured dependency injection for services, repositories, and MediatR handlers.
- **Custom Controller Base Class**: Simplified controller response handling with result patterns.

---

## Installation

1. **Install the Template**

   Install the template directly from NuGet:

   ```bash
   dotnet new -i TalMoDa.Custom.Template
   ```

2. **Create a New Project Using the Template**

   After installing, create a new project by running:

   ```bash
   dotnet new TalDayanCustomTemplate -n YourProjectName
   ```

3. **Available Parameters**

   You can customize various aspects of the generated project using parameters. Here are all the available parameters:

   | Parameter                         | Description                                         | Default Value                                                |
   |-----------------------------------|-----------------------------------------------------|--------------------------------------------------------------|
   | `Framework`                       | The target framework for the project                | `net6.0`                                                     |
   | `DbContextName`                   | The name of the DbContext used in the project       | `AppDbContext`                                               |
   | `DbName`                          | The name of the database                            | `MyDb`                                                       |
   | `ConnectionStrings__DefaultConnection` | The connection string for the database        | `Server=(LocalDB)\\MSSQLLocalDB;Database={MyDb};Trusted_Connection=True;MultipleActiveResultSets=true` |
   | `MyCustomControllerName`          | The name of the custom controller                   | `MyCustomTemplateController`                                 |

   ### Example Usage with Parameters

   To create a project with custom values for each parameter:

   ```bash
   dotnet new TalDayanCustomTemplate -n YourProjectName --Framework net6.0 --DbContextName CustomDbContext --DbName CustomDb --ConnectionStrings__DefaultConnection "Server=(LocalDB)\\MSSQLLocalDB;Database=CustomDb;Trusted_Connection=True;MultipleActiveResultSets=true" --MyCustomControllerName CustomController
   ```

4. **Configure Database Connection**

   The template includes placeholders for database configuration. Set up your connection string and database name:

   ```powershell
   $connectionString = '"{SQL_CONNECTION_STRING}"'
   $provider = "Microsoft.EntityFrameworkCore.SqlServer"
   ```

---

## Usage Examples

### Repository Usage

The template includes a base repository (`IBaseRepository`) and an example repository (`IExampleRepository`). Here’s how to use it:

```csharp
public class ExampleService
{
    private readonly IExampleRepository _exampleRepository;

    public ExampleService(IExampleRepository exampleRepository)
    {
        _exampleRepository = exampleRepository;
    }

    public async Task<ExampleDto> GetExample(int id, CancellationToken cancellationToken)
    {
        var example = await _exampleRepository.GetExampleAsNoTrackingAsync(id, cancellationToken);
        if (example == null)
        {
            throw new NotFoundException($"Example with ID {id} not found.");
        }
        return new ExampleDto(example.Id, example.Name);
    }
}
```

### CQRS Pattern with MediatR

The template leverages MediatR to handle queries. Here’s an example of a query for retrieving an example entity:

```csharp
public record GetExampleQuery(int Id) : IRequest<Result<ExampleDto>>;

public class GetExampleQueryHandler : IRequestHandler<GetExampleQuery, Result<ExampleDto>>
{
    private readonly IExampleRepository _exampleRepository;

    public GetExampleQueryHandler(IExampleRepository exampleRepository)
    {
        _exampleRepository = exampleRepository;
    }

    public async Task<Result<ExampleDto>> Handle(GetExampleQuery request, CancellationToken cancellationToken)
    {
        var example = await _exampleRepository.GetExampleAsNoTrackingAsync(request.Id, cancellationToken);
        
        if (example is null)
        {
            return Error.NotFound($"Example with ID {request.Id} was not found.");
        }

        return new ExampleDto(example.Id, example.Name);
    }
}
```

### Custom Controller Example

The template includes `AppBaseController` to simplify response handling with a standardized result pattern:

```csharp
[Route("[controller]")]
public class MyCustomTemplateController : AppBaseController
{
    private readonly IMediator _mediator;

    public MyCustomTemplateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("/example/{id}")]
    public async Task<IActionResult> GetExample(int id, CancellationToken cancellationToken)
    {
        var query = await _mediator.Send(new GetExampleQuery(id), cancellationToken);
        return ResultOf(query);
    }
}
```

### Swagger Documentation

This template includes Swagger for API documentation. Access the Swagger UI at `/swagger` when the application is running in development mode.

---

## Dependencies

- **Entity Framework Core**: For data access.
- **MediatR**: For CQRS pattern.
- **FluentValidation**: For validating requests.
- **Serilog**: For structured logging.
- **Swashbuckle.AspNetCore**: For Swagger API documentation.

---

## PowerShell Script Details

The included PowerShell script automates database scaffolding and namespace updating. Key steps include:

1. **Scaffolding Entities**: Generates entities based on your configured connection string and provider.
2. **Updating Namespaces**: Adjusts namespaces in generated files to align with the template's structure.
3. **File Management**: Moves generated files from a temporary folder to the main folder.

To run the script:

```powershell
.\scaffold.ps1
```

