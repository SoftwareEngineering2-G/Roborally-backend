using FastEndpoints;
using FastEndpoints.Swagger;
using Roborally.core.application;
using Roborally.infrastructure.persistence;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");

builder.Services.InstallPersistenceModule(connectionString);


builder.Services.AddFastEndpoints().SwaggerDocument();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.Services.RegisterApplicationModule();

app.UseHttpsRedirection();
app.UseDefaultExceptionHandler().UseFastEndpoints(c => { c.Endpoints.RoutePrefix = "api"; }).UseSwaggerGen();

app.Run();