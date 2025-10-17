using DotNetEnv;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using MinibusTracker.Application.Abstractions.Persistence;
using MinibusTracker.Application.Common.Interfaces;
using MinibusTracker.Infrastructure.Common.Logging;
using MinibusTracker.Infrastructure.Persistence.Common;
using MinibusTracker.Infrastructure.Persistence.Repositories;
using MinibusTracker.Application.Abstractions.Data;
using MinibusTracker.Application.UseCases.Associations;


var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    // My ENV stays on the root
    var envPath = Path.Combine(builder.Environment.ContentRootPath, "..", ".env");
    Env.Load(envPath);

    var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
    var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
    var dbName = Environment.GetEnvironmentVariable("DB_NAME");
    var dbUser = Environment.GetEnvironmentVariable("DB_USER");
    var dbPass = Environment.GetEnvironmentVariable("DB_PASS");
    var dbSsl = Environment.GetEnvironmentVariable("DB_SSL");
    string databaseConnectionString =
    $"Server={dbHost};Port={dbPort};Database={dbName};User Id={dbUser};Password={dbPass};SslMode={dbSsl};GuidFormat=Char36";

    builder.Configuration["ConnectionStrings:TaxiDb"] = databaseConnectionString;
}

// Configure API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// ensures a MySqlConnectionFactory has only one instance throughout the application's lifecycle
builder.Services.AddSingleton<IDBConnectionFactory, MySqlConnectionFactory>();

// Registrations 

// ASSOCIATION
// Repository and SERVICES
builder.Services.AddScoped<IAssociationRepository, AssociationRepository>();
builder.Services.AddScoped<IAssociationService, AssociationService>();


// Configure logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole(); 
});
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
