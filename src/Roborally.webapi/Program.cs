using FastEndpoints;
using FastEndpoints.Swagger;
using Roborally.core.application;
using Roborally.infrastructure.persistence;
using Roborally.webapi.Middleware;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.InstallPersistenceModule(connectionString);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


builder.Services.AddFastEndpoints().SwaggerDocument();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddProblemDetails();
var app = builder.Build();
app.Services.RegisterApplicationModule();


app.UseHttpsRedirection();
app.UseCors();
app.UseFastEndpoints(c => {
    c.Endpoints.RoutePrefix = "api";
    c.Endpoints.Configurator = ep => {
        ep.AllowAnonymous();
        ep.DontCatchExceptions();
    };
}).UseSwaggerGen();
app.UseExceptionHandler();

app.Run();