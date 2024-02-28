using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;
using TicketsRUs.WebApp.Components;
using TicketsRUs.WebApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddSingleton<ITicketService, ApiTicketService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
//builder.Services.AddSingleton(sp => RabbitMQFactory.CreateBus(BusType.LocalHost));

// Logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(Configuration)
    .WriteTo.OpenTelemetry(options =>
    {
        options.Endpoint = $"{Configuration.GetValue<string>("Otlp:Endpoint")}/v1/logs";
        options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc;
        options.ResourceAttributes = new Dictionary<string, object>
        {
            ["service.name"] = Configuration.GetValue<string>("Otlp:ServiceName")
        };
    })
    .CreateLogger();

// Tracing
Action<ResourceBuilder> appResourceBuilder =
    resource => resource
        .AddTelemetrySdk()
        .AddService(Configuration.GetValue<string>("Otlp:ServiceName"));

builder.Services.AddOpenTelemetry()
    .ConfigureResource(appResourceBuilder)
    .WithTracing(builder => builder
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSource("APITracing")
        //.AddConsoleExporter()
        .AddOtlpExporter(options => options.Endpoint = new Uri(Configuration.GetValue<string>("Otlp:Endpoint")))
    )
    .WithMetrics(builder => builder
        .AddRuntimeInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter(options => options.Endpoint = new Uri(Configuration.GetValue<string>("Otlp:Endpoint"))));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health Check
builder.Services.AddHealthChecks();

builder.Services.AddDbContextFactory<PostgresContext>(options => options.UseNpgsql("Name=db"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

app.Run();
