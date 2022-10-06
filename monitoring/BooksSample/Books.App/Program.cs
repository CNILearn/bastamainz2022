
using Books.App;

using OpenTelemetry;
using OpenTelemetry.Trace;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddHttpClientInstrumentation()
    .AddAspNetCoreInstrumentation()
    .AddSource("Books.App")
    .AddJaegerExporter()
    .AddConsoleExporter()
    .Build();

var builder = WebApplication.CreateBuilder(args);
bool isRunningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
if (isRunningInContainer)
{
    builder.Configuration.AddJsonFile("appsettings.Docker.json");
}

builder.Services.AddRazorPages();
builder.Services.AddHttpClient<BooksClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BooksApi"]);
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();