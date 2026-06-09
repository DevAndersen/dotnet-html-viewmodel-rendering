using ConsoleApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.Services
    .AddLogging(k => k.ClearProviders()) // Removes logging, not of interest in this proof-of-concept.
    .AddScoped<HtmlRenderer>()
    .AddScoped<HtmlRenderingService>()
    .AddHostedService<HostedService>();

IHost app = builder.Build();
app.Run();
