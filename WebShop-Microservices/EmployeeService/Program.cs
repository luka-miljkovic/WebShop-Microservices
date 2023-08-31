using EmployeeService;
using EmployeeService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

/* Database Context Dependency Injection */
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_ROOT_PASSWORD");
//var dbHost = "localhost";
//var dbName = "dms_employees";
//var dbPassword = "admin";
var connectionString = $"server={dbHost};port=3306;database={dbName};user=root;password={dbPassword}";
builder.Services.AddDbContext<EmployeeDbContext>(o => o.UseMySQL(connectionString));
/* ===================================== */

//adding grpc service
builder.Services.AddGrpc();

//builder.WebHost.ConfigureKestrel(options =>
//{
//    // Setup a HTTP/2 endpoint without TLS.
//    options.ListenLocalhost(5076, o => o.Protocols =
//        HttpProtocols.Http2);
//});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGrpcService<GrpcEmployeeService>();

    endpoints.MapGet("/protos/employees.proto", async context =>
    {
        await context.Response.WriteAsync(File.ReadAllText("Protos/employees.proto"));
    });
});

PrepDb.PrepPopulation(app);

app.Run();
