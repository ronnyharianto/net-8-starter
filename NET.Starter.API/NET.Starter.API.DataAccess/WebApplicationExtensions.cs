using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NET.Starter.API")]

namespace NET.Starter.API.DataAccess
{
    internal static class WebApplicationExtensions
    {
        public static WebApplication UseDbContext(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
            dbContext.Database.EnsureCreated();

            return app;
        }
    }
}
