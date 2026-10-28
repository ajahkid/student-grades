using BicycleService.Milestone04.Database.Data;
using BicycleService.Milestone04.Database.Presentation;
using BicycleService.Milestone04.Database.Services;
using Microsoft.EntityFrameworkCore;

string? configuredDatabasePath = Environment.GetEnvironmentVariable("BICYCLE_SERVICE_DB_PATH");
string databasePath = string.IsNullOrWhiteSpace(configuredDatabasePath)
	? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BicycleService", "Milestone04", "bicycle-service.db")
	: configuredDatabasePath;
string? databaseDirectory = Path.GetDirectoryName(databasePath);
if (!string.IsNullOrEmpty(databaseDirectory))
{
	Directory.CreateDirectory(databaseDirectory);
}

DbContextOptions<BicycleServiceDbContext> options = new DbContextOptionsBuilder<BicycleServiceDbContext>()
	.UseSqlite($"Data Source={databasePath}")
	.Options;

using BicycleServiceDbContext database = new(options);
database.Database.EnsureCreated();

AuthenticationService authenticationService = new(database);
ServiceJobService serviceJobService = new(database);
ConsoleApplication application = new(authenticationService, serviceJobService);
application.Run();