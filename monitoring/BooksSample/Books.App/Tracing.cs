using OpenTelemetry;
using OpenTelemetry.Trace;

using System.Diagnostics;

namespace Books.App;

internal class Tracing
{
    public static TracerProvider? ConfigureTracing()
    {
        return Sdk.CreateTracerProviderBuilder()
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddSource("Books.App")
            .AddJaegerExporter()
            .AddConsoleExporter()
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