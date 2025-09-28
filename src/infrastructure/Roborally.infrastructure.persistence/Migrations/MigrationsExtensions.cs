using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Roborally.infrastructure.persistence.Migrations;

public static class MigrationsExtensions {
    public static async Task<IApplicationBuilder> ApplyMigrations(this IApplicationBuilder app) {
        using var scope = app.ApplicationServices.CreateScope();
        await using AppDatabaseContext context = scope.ServiceProvider.GetRequiredService<AppDatabaseContext>();
        if (context.Database.IsRelational()) {
            await context.Database.MigrateAsync();
        }

        return app;
    }
}