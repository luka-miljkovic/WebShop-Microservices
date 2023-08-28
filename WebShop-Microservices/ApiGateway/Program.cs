using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

//adding dependency injection for Ocelot Api gateway
//we need to create a json configuration file for ocelot api gateway. 
//providing method for adding the json file to the conf
//assigning the base path to the content root folder and than providing json file 

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables(); // read conf values from the env varieables whith a specific prefix
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
await app.UseOcelot();


app.Run();
