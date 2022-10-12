using Azure.Monitor.OpenTelemetry.Exporter;

using OpenTelemetry;
using OpenTelemetry.Trace;

using System.Diagnostics;

namespace Books.API;

internal class Tracing
{
    public static TracerProvider? ConfigureTracing(IConfiguration settings)
    {
        return Sdk.CreateTracerProviderBuilder()
            .AddAspNetCoreInstrumentation()
            .AddSource("Books.App")
            .AddConsoleExporter()
            .AddAzureMonitorTraceExporter(options =>
            {
                string connectionString = settings["AppInsightsConnectionString"] ?? throw new InvalidOperationException("AppInsightsConnectionString not found");
                options.ConnectionString = connectionString;
            })
            .Build();
    }

    private static ActivitySource? s_activitySource;

    public static ActivitySource ActivitySource 
    {
        get => s_activitySource ??= new ActivitySource("Books.API");
    }

    public static Activity? StartGetBooksActivity(HttpRequest request)
    {
        var activity = ActivitySource.StartActivity("GetBooks", ActivityKind.Server);
        activity?.SetBaggage("Query", request.QueryString.Value);
        activity?.AddEvent(new ActivityEvent("GetBooksStart"));
        return activity;
    }

    public static void CompleteGetBooks(Activity? activity)
    {
        activity?.AddEvent(new ActivityEvent("GetBooksComplete"));
    }
}
