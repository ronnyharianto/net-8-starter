using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace NET.Starter.API.DataAccess
{
    public static class WebApplicationExtensions
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
