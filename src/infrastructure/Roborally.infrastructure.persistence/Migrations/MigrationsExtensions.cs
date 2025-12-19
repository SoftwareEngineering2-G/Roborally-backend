using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Roborally.infrastructure.persistence.Migrations;

public static class MigrationsExtensions {
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 8" />
    public static async Task<IApplicationBuilder> ApplyMigrations(this IApplicationBuilder app) {
        using var scope = app.ApplicationServices.CreateScope();
        await using AppDatabaseContext context = scope.ServiceProvider.GetRequiredService<AppDatabaseContext>();
        if (context.Database.IsRelational()) {
            await context.Database.MigrateAsync();
        }

        return app;
    }
}