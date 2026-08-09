using TrainingB.Core.Configuration;
using TrainingB.Core.Services;
using CoreConfigManager = TrainingB.Core.Configuration.ConfigurationManager;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // Disable file watching in production to avoid inotify limit issues on Render
    ContentRootPath = AppContext.BaseDirectory
});

// Disable file change monitoring for configuration files
builder.Configuration.Sources
    .OfType<Microsoft.Extensions.Configuration.Json.JsonConfigurationSource>()
    .ToList()
    .ForEach(s => s.ReloadOnChange = false);

// Configure Kestrel server with extended timeouts for long-running scrapers
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    // Increase timeouts to handle scrapers that take 3-5 minutes
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(2);
});

// Load configuration
CoreConfigManager.Initialize(builder.Configuration);
Logger.Info("TrainingB.Web starting...");

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for development
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

// Serve static files (PWA UI)
app.UseDefaultFiles();
app.UseStaticFiles();

// API endpoints
app.MapControllers();

// Health check endpoint for Render.com (prevents auto-restart during long scraping operations)
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

// Fallback to index.html for SPA
app.MapFallbackToFile("index.html");

Logger.Info($"TrainingB.Web started on {app.Urls.FirstOrDefault() ?? "unknown"}");

app.Run();
