using Microsoft.AspNetCore.Builder;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var serviceName = "MyCompany.MyProduct.MyService";
var serviceVersion = "1.0.0";

var appBuilder = WebApplication.CreateBuilder(args);

appBuilder.Services.AddOpenTelemetry()
    .WithTracing(builder => builder
        .AddConsoleExporter()
        .AddSource(serviceName)
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation())
    .WithMetrics(builder => builder
        .AddConsoleExporter()
        // .AddMeter(meter.Name)
        // .SetResourceBuilder(appResourceBuilder)
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation())
    .StartWithHost();

var app = appBuilder.Build();

app.MapGet("/", () => "Hello World");

app.Run();