using DeliveryService;
using DeliveryService.AsyncDataService;
using DeliveryService.EventProcessing;
using DeliveryService.SyncDataService.Grpc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

/* Database Context Dependency Injection */
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_ROOT_PASSWORD");
//var dbHost = "localhost";
//var dbName = "dbs_deliveries";
//var dbPassword = "admin";
var connectionString = $"server={dbHost};port=3306;database={dbName};user=root;password={dbPassword}";
builder.Services.AddDbContext<DeliveryDbContext>(o => o.UseMySQL(connectionString));
/* ===================================== */

//add service for gRPC

builder.Services.AddScoped<IEmployeeDataClient, EmployeeDataClient>();

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddHostedService<MessageBusSubscriber>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
