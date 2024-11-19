using NET.Starter.API.Core;
using NET.Starter.API.DataAccess;
using NET.Starter.API.Shared;

var builder = WebApplication.CreateBuilder(args);

// Register Dependency Injection
builder.Services.RegisterShared();
builder.Services.RegisterDataAccess(builder.Configuration);
builder.Services.RegisterCore();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use from Web Application Extensions
app.UseDbContext();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
