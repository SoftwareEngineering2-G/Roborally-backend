using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Roborally.core.application;
using Roborally.core.application.ApplicationContracts.GameTimer;
using Roborally.core.application.Services;
using Roborally.infrastructure.persistence;
using Roborally.infrastructure.broadcaster;
using Roborally.infrastructure.persistence.Migrations;
using Roborally.infrastructure.persistence.Game;
using Roborally.webapi.Middleware;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not found.");

// Get JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");

builder.Services.AddCors(options => {
    options.AddPolicy("FrontendCorsPolicy", policy => {
        policy.WithOrigins(
                "http://130.225.71.179:3000", // VM
                "http://localhost:3000"        // Local dev
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.InstallPersistenceModule(connectionString, builder.Configuration);
builder.Services.InstallBroadcasterModule();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero // No tolerance for expired tokens
    };
});

// Add authorization services
builder.Services.AddAuthorization();

builder.Services.AddFastEndpoints().SwaggerDocument();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddProblemDetails();
builder.Services.AddScoped<IProgrammingTimeoutHandler, ProgrammingTimeoutHandler>();
var app = builder.Build();
app.Services.RegisterApplicationModule();

if (app.Environment.IsDevelopment()) {
    await app.ApplyMigrations();
    
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<GameBoardSeeder>();
    await seeder.SeedBoardsAsync();
}

app.UseHttpsRedirection();
app.UseCors("FrontendCorsPolicy");

// Use authentication and authorization middleware (BEFORE FastEndpoints!)
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(c => {
    c.Endpoints.RoutePrefix = "api";
    c.Endpoints.Configurator = ep => {
        // Authentication required by default
        // Individual endpoints can opt-out with AllowAnonymous()
        ep.DontCatchExceptions();
    };
}).UseSwaggerGen();

app.InstallBroadcasterModule();
app.UseExceptionHandler();

app.Run();