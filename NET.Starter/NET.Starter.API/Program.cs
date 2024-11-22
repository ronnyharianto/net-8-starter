using NET.Starter.Core;
using NET.Starter.DataAccess.SqlServer;
using NET.Starter.API.Extensions.StartupExtensions;
using NET.Starter.Shared;
using Serilog;
using NET.Starter.SDK.Managers;

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

var zebraRfidReaderManager = app.Services.GetRequiredService<ZebraRfidReaderManager>();
app.Lifetime.ApplicationStopping.Register(zebraRfidReaderManager.Dispose);

app.Run();
