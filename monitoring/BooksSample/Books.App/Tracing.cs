using Azure.Monitor.OpenTelemetry.Exporter;

using OpenTelemetry;
using OpenTelemetry.Trace;

using System.Diagnostics;

namespace Books.App;

internal class Tracing
{
    public static TracerProvider? ConfigureTracing(IConfiguration settings)
    {
        return Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation()
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
        get => s_activitySource ??= new ActivitySource("Books.App", "2.0");
    }
    
    public static Activity? StartGetBooksActivity()
    {
        return ActivitySource.StartActivity("GetBooksClient", ActivityKind.Client);
    }
}

internal static class ActivityExtensions
{
    public static Activity? StartGetBooksEvent(this Activity? activity, HttpClient httpClient)
    {
        activity?.SetBaggage("BaseUrl", httpClient.BaseAddress?.ToString());
        activity?.AddEvent(new ActivityEvent("RequestGetBooksEvent"));
        return activity;
    }

    public static Activity? CompleteGetBooksEvent(this Activity? activity)
    {
        activity?.AddEvent(new ActivityEvent("GetBooksReceivedEvent"));
        return activity;
    }
}