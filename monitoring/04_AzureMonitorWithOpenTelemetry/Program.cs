global using Microsoft.Extensions.Logging;

global using System.Diagnostics;

using Azure.Monitor.OpenTelemetry.Exporter;

using DiagnosticsSample;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;



using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddUserSecrets("a10ae4e6-96f7-4dbc-a6de-00ceebe569fc");
    })
    .ConfigureLogging(logging =>
    {
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
        services.AddTransient<Runner>();
    })
    .Build();

Dictionary<string, object> resourceAttributes = new() {
    { "service.name", "my-diagnosticssample" },
    { "service.namespace", "my-diagnosticssample-namespace" },
    { "service.instance.id", "my-diagnosticssample-instance" }};
var resourceBuilder = ResourceBuilder.CreateDefault().AddAttributes(resourceAttributes);

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(resourceBuilder)
    .AddSource("DiagnosticsSample")
    .AddAzureMonitorTraceExporter(o =>
    {
        IConfiguration settings = host.Services.GetRequiredService<IConfiguration>();
        string connectionString = settings["AppInsightsConnectionString"] ?? throw new InvalidOperationException("AppInsightsConnectionString not found");
        o.ConnectionString = connectionString;
    })
    .Build();

var runner = host.Services.GetRequiredService<Runner>();

for (int i = 0; i < 200; i++)
{
    runner.InfoMessage1();
    runner.InfoMessage2();
    runner.ErrorMessage();
    await Task.Delay(2000);
    runner.Foo();
    runner.AddARecord();
}
