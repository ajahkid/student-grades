using BicycleService.Milestone03.LayeredConsole.Presentation;
using BicycleService.Milestone03.LayeredConsole.Repositories;
using BicycleService.Milestone03.LayeredConsole.Services;

string? configuredDataDirectory = Environment.GetEnvironmentVariable("BICYCLE_SERVICE_DATA_DIRECTORY");
string dataDirectory = string.IsNullOrWhiteSpace(configuredDataDirectory)
	? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BicycleService", "Milestone03")
	: configuredDataDirectory;
IUserRepository userRepository = new JsonUserRepository(dataDirectory);
IServiceJobRepository serviceJobRepository = new JsonServiceJobRepository(dataDirectory);
AuthenticationService authenticationService = new(userRepository);
ServiceJobService serviceJobService = new(serviceJobRepository);
ConsoleApplication application = new(authenticationService, serviceJobService);

application.Run();