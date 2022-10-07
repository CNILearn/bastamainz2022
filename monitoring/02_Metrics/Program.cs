global using Microsoft.Extensions.Logging;

global using System.Diagnostics;

using DiagnosticsSample;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;



using var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.AddJsonConsole(config =>
        {
            config.JsonWriterOptions = new JsonWriterOptions() { Indented = true };
        });
        logging.SetMinimumLevel(LogLevel.Trace);
    })
    .ConfigureServices((context, services) =>
    {
        string connectionString = context.Configuration.GetConnectionString("BooksConnection");
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
