using NET.Starter.API.Core;
using NET.Starter.API.DataAccess;
using NET.Starter.API.Extensions.StartupExtensions;
using NET.Starter.API.Shared;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterShared(builder.Host, builder.Configuration);
builder.AddController();
builder.AddSwaggerGen();
builder.AddCors();
builder.Services.RegisterDataAccess(builder.Configuration);
builder.Services.RegisterCore(builder.Configuration);
builder.AddAuthentication();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseDbContext();
app.UseHttpLogging().UseSerilogRequestLogging();

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

app.Run();
