using System.Text.Json;
using FastEndpoints;
using FastEndpoints.Swagger;
using Roborally.core.application;
using Roborally.core.domain.Game.Gameboard.Space;
using Roborally.infrastructure.persistence;
using Roborally.infrastructure.broadcaster;
using Roborally.infrastructure.persistence.Migrations;
using Roborally.webapi.Middleware;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    opts.JsonSerializerOptions.Converters.Add(new SpaceObjectJsonConverter());
});




builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000") // Add your frontend URLs
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); 
    });
});

builder.Services.InstallPersistenceModule(connectionString);
builder.Services.InstallBroadcasterModule(); // Register SignalR services
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


builder.Services.AddFastEndpoints().SwaggerDocument();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddProblemDetails();
var app = builder.Build();
app.Services.RegisterApplicationModule();

if (app.Environment.IsDevelopment()) {
    await app.ApplyMigrations();
}


app.UseHttpsRedirection();
app.UseCors();
app.UseFastEndpoints(c => {
    c.Endpoints.RoutePrefix = "api";
    c.Endpoints.Configurator = ep => {
        ep.AllowAnonymous();
        ep.DontCatchExceptions();
    };
}).UseSwaggerGen();

app.InstallBroadcasterModule(); // Map the SignalR hub to match REST endpoint pattern
app.UseExceptionHandler();

app.Run();