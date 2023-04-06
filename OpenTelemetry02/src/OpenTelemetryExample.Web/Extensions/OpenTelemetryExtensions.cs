using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;

namespace OpenTelemetry.Web.Extensions;


public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddTelemetryInstrumentation(this IServiceCollection services)
    {
        static void configureResource(ResourceBuilder r) => r.AddService(
            serviceName: "OpenTelemetry-Web",
            serviceVersion: Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown",
            serviceInstanceId: Environment.MachineName);



        services.AddOpenTelemetry()
            .ConfigureResource(configureResource)
            .WithTracing(tracerProviderBuilder => tracerProviderBuilder
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddJaegerExporter(options =>
                {
                    // options.AgentHost = "jaeger";
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

        return services;
    }
}