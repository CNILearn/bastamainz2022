global using Microsoft.Extensions.Logging;

global using System.Diagnostics;

using Azure.Monitor.OpenTelemetry.Exporter;

using DiagnosticsSample;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

const string ServiceName = "DiagnosticsSample";

Dictionary<string, object> resourceAttributes = new() {
    { "service.name", "my-diagnosticssample" },
    { "service.namespace", "my-diagnosticssample-namespace" },
    { "service.instance.id", "my-diagnosticssample-instance" }};

ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault()
    .AddService(ServiceName);



using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddUserSecrets("a10ae4e6-96f7-4dbc-a6de-00ceebe569fc");
    })
    .ConfigureLogging(logging =>
    {
        logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.SetResourceBuilder(resourceBuilder);
        });
        //logging.AddJsonConsole(config =>
        //{
        //    config.JsonWriterOptions = new JsonWriterOptions() { Indented = true };
        //});
        logging.SetMinimumLevel(LogLevel.Trace);
    })
    .ConfigureServices((context, services) =>
    {
        string connectionString = context.Configuration.GetConnectionString("BooksConnection") ?? throw new InvalidOperationException("Connection string not found");
        services.AddDbContextFactory<BooksContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddOpenTelemetryTracing(builder =>
        {
            builder
            .AddSource(ServiceName)
            .AddConsoleExporter()
            .AddAzureMonitorTraceExporter(options =>
            {
                IConfiguration settings = context.Configuration;
                string connectionString = settings["AppInsightsConnectionString"] ?? throw new InvalidOperationException("AppInsightsConnectionString not found");
                options.ConnectionString = connectionString;
            })
            .AddSource(ServiceName)
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(ServiceName)
                    .AddAttributes(resourceAttributes))
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddSqlClientInstrumentation();

        });
        services.AddOpenTelemetryMetrics(builder =>
        {
            builder.SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(ServiceName))
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter(options =>
            {
            });
        });
        services.AddTransient<Runner>();
    })
    .Build();

using var tracer = Sdk.CreateTracerProviderBuilder()
    .AddAspNetCoreInstrumentation()
    .AddSource(ServiceName)
    .AddConsoleExporter()
    .AddAzureMonitorTraceExporter(options =>
    {        
        IConfiguration settings = host.Services.GetRequiredService<IConfiguration>();
        string connectionString = settings["AppInsightsConnectionString"] ?? throw new InvalidOperationException("AppInsightsConnectionString not found");
        options.ConnectionString = connectionString;
    })
    .Build();



var runner = host.Services.GetRequiredService<Runner>();

for (int i = 0; i < 200; i++)
{
    runner.InfoMessage1();
    runner.InfoMessage2();
    runner.ErrorMessage();
    await Task.Delay(200);
    runner.Foo();
    runner.AddARecord();
}
