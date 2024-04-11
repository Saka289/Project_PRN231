using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Web.GatewaySolution.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddAppAuthencation();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseOcelot().GetAwaiter().GetResult();

app.Run();
