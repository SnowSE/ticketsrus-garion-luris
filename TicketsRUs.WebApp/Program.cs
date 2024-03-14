using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;
using TicketsRUs.WebApp.Components;
using TicketsRUs.WebApp.Services;
using TicketsRUs.WebApp.Telemetry.Traces;
using TicketsRUs.WebApp.Telemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddSingleton<ITicketService, ApiTicketService>();
builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.AddHealthChecks();
builder.Services.AddLogging();

const string serviceName = "tickets";
const string serviceVersion = "1.0.0";

builder.Services.AddOpenTelemetry()
.ConfigureResource(resource => resource.AddService(serviceName: serviceName, serviceVersion: serviceVersion))
.WithMetrics(metrics =>
{
    metrics.AddAspNetCoreInstrumentation();
    metrics.AddMeter(Metrics.Name);
    metrics.AddConsoleExporter();

    metrics.AddOtlpExporter(o =>
    {
        o.Endpoint = new Uri("http://otel-collector:4317/");
    });
})
.WithTracing(b =>
{
    b
    .AddSource(serviceName)
    .AddSource(Traces.Name)
    .AddAspNetCoreInstrumentation()
    .AddConsoleExporter()
    .AddOtlpExporter(o =>
    {
        o.Endpoint = new Uri("http://otel-collector:4317/");
    })
    .AddZipkinExporter(o =>
    {
        o.Endpoint = new Uri("http://zipkin:9411/");
    });
});

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddOtlpExporter(o =>
        {
            o.Endpoint = new Uri("http://otel-collector:4317/");
        });
        //.AddConsoleExporter();
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health Check

builder.Services.AddDbContextFactory<PostgresContext>(options => options.UseNpgsql("Name=db"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

// Swagger Components
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//app.MapPrometheusScrapingEndpoint();

app.Run();

public partial class Program { }