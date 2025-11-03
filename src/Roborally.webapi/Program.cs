using FastEndpoints;
using FastEndpoints.Swagger;
using Roborally.core.application;
using Roborally.infrastructure.persistence;
using Roborally.infrastructure.broadcaster;
using Roborally.infrastructure.persistence.Migrations;
using Roborally.infrastructure.persistence.Game;
using Roborally.webapi.Middleware;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");

builder.Services.AddCors(options => {
    options.AddPolicy("FrontendCorsPolicy", policy => {
        policy.WithOrigins(
                "http://130.225.71.179:3000", // VM
                "http://localhost:3000" // Local dev
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // ✅ required for SignalR
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
    
    // Seed game boards from JSON files
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<GameBoardSeeder>();
    await seeder.SeedBoardsAsync();
}

app.UseHttpsRedirection();
app.UseCors("FrontendCorsPolicy");
app.UseFastEndpoints(c => {
    c.Endpoints.RoutePrefix = "api";
    c.Endpoints.Configurator = ep => {
        ep.AllowAnonymous();
        ep.DontCatchExceptions();
    };
}).UseSwaggerGen();

app.InstallBroadcasterModule();
app.UseExceptionHandler();

app.Run();