using BicycleService.Milestone05.WebAPI.Data;
using BicycleService.Milestone05.WebAPI.Endpoints;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
string? configuredDatabasePath = Environment.GetEnvironmentVariable("BICYCLE_SERVICE_API_DB_PATH");
string databasePath = string.IsNullOrWhiteSpace(configuredDatabasePath)
	? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BicycleService", "Milestone05", "bicycle-service.db")
	: configuredDatabasePath;
string? databaseDirectory = Path.GetDirectoryName(databasePath);
if (!string.IsNullOrEmpty(databaseDirectory))
{
	Directory.CreateDirectory(databaseDirectory);
}

builder.Services.AddDbContext<BicycleServiceDbContext>(options => options.UseSqlite($"Data Source={databasePath}"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();
using (IServiceScope scope = app.Services.CreateScope())
{
	scope.ServiceProvider.GetRequiredService<BicycleServiceDbContext>().Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapServiceJobEndpoints();

app.Run();