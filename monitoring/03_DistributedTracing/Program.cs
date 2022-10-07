global using Microsoft.Extensions.Logging;

global using System.Diagnostics;

using DiagnosticsSample;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("DiagnosticsSample"))
    .AddSource("DiagnosticsSample")
    .AddConsoleExporter()
    .Build();

using var host = Host.CreateDefaultBuilder(args)
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
        string connectionString = context.Configuration.GetConnectionString("BooksConnection") ?? throw new InvalidOperationException("can't read BooksConnection");
        services.AddDbContextFactory<BooksContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddTransient<Runner>();
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
