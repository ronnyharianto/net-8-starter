using NET.Starter.API.Extensions.StartupExtensions;
using NET.Starter.Core;
using NET.Starter.DataAccess;
using NET.Starter.Shared;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var corsOrigin = builder.Configuration.GetValue<string>("CorsOrigin")?.Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray();

builder.Services.RegisterShared(builder.Host, builder.Configuration);
builder.Services.RegisterDataAccess(builder.Configuration);
builder.Services.RegisterCore();

builder.AddController();
builder.AddSwaggerGen();
builder.AddCors(corsOrigin);
builder.AddAuthentication();

var app = builder.Build();

app.UseHttpLogging().UseSerilogRequestLogging();

await app.UseDbContext();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "NET.Starter.API";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

app.UseCors();
app.MapControllers();

app.UseAntiforgery();

app.Run();


app.Run();
