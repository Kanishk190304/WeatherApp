using LoggingService.Application.Interfaces;
using LoggingService.Domain.Interfaces;
using LoggingService.Infrastructure.Data;
using LoggingService.Infrastructure.Repositories;
using LoggingService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Register MongoDB context (Singleton - one connection for app lifetime)
builder.Services.AddSingleton<MongoDbContext>();

// Register repository (Scoped - one per request)
builder.Services.AddScoped<ILogRepository, LogRepository>();

// Register logging service (Scoped - to match repository lifetime)
builder.Services.AddScoped<ILoggingService, LoggingServiceImpl>();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();