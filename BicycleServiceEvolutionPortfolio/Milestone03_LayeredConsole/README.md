# Milestone 3: Layered Multi-user Console

**Status:** Complete

This version separates the console presentation, service rules, and JSON file storage. Users can register, log in, and log out. Each account sees only the service jobs stored in its own file.

The service menu supports adding, listing, finding, updating, and deleting jobs. Registration validates usernames and requires passwords to be at least eight characters. Passwords are stored as salted PBKDF2 hashes rather than plain text.

Account details and service records are stored under the current user's Local AppData folder, in `BicycleService/Milestone03`. Each user has a separate `user-{id}-customers.json` data file. This keeps local account data outside the repository.

For testing, set the `BICYCLE_SERVICE_DATA_DIRECTORY` environment variable to use a different data folder.

## Run Milestone 3

From the repository root, run:

```powershell
dotnet run --project BicycleServiceEvolutionPortfolio/Milestone03_LayeredConsole/BicycleService.Milestone03.LayeredConsole.csproj
```