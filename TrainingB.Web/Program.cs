using TrainingB.Core.Configuration;
using TrainingB.Core.Services;
using CoreConfigManager = TrainingB.Core.Configuration.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

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

// Fallback to index.html for SPA
app.MapFallbackToFile("index.html");

Logger.Info($"TrainingB.Web started on {app.Urls.FirstOrDefault() ?? "unknown"}");

app.Run();
