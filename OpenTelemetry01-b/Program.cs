using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

// Ayudas:
// https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/examples/AspNetCore/Program.cs
// https://github.com/davidfowl/TodoApi/blob/main/TodoApi/Extensions/OpenTelemetryExtensions.cs


var builder = WebApplication.CreateBuilder(args);

Action<ResourceBuilder> configureResource = r => r.AddService(
    serviceName: builder.Configuration.GetValue<string>("ServiceName"),
    serviceVersion: Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown",
    serviceInstanceId: Environment.MachineName);

builder.Services.AddOpenTelemetry()
    .ConfigureResource(configureResource)
    .WithTracing(tracerProviderBuilder => tracerProviderBuilder
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .AddJaegerExporter(options => 
        {
            options.AgentHost = "jaeger";
        }))
    .WithMetrics(metricProviderBuilder => metricProviderBuilder
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEventCountersInstrumentation(c =>
        {
            // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/available-counters
            c.AddEventSources(
                "Microsoft.AspNetCore.Hosting",
                "Microsoft-AspNetCore-Server-Kestrel",
                "System.Net.Http",
                "System.Net.Sockets",
                "System.Net.NameResolution",
                "System.Net.Security");
        })
        .AddPrometheusExporter())      
    .StartWithHost();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddOpenTelemetry(options =>
{
    var resourceBuilder = ResourceBuilder.CreateDefault();
    configureResource(resourceBuilder);

    options
        .SetResourceBuilder(resourceBuilder)
        .AddOtlpExporter();
});


var app = builder.Build();

app.MapGet("/", () => "Welcome");
app.MapGet("/hello", (ILogger<Program> log) => 
{
    log.LogInformation("Starting hello");

    return new 
    {
        Gretting = "Hello"
    };
});

app.MapPrometheusScrapingEndpoint();
app.Run();