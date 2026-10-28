# Milestone 4: Database Application

**Status:** Complete

This version keeps the account-based service workflow and stores users, customers, bicycles, and service jobs in related SQLite tables using EF Core. The console app supports registration, login/logout, Add/List/Find/Update/Delete, and keeps each user's records separate.

The database schema is created with EF Core's `EnsureCreated` when the app starts. This version does not include EF Core migrations.

By default, the database is stored under the current user's Local AppData folder in `BicycleService/Milestone04/bicycle-service.db`. For testing, set the `BICYCLE_SERVICE_DB_PATH` environment variable to a different SQLite file path.

## Run Milestone 4

From the repository root, run:

```powershell
dotnet run --project BicycleServiceEvolutionPortfolio/Milestone04_Database/BicycleService.Milestone04.Database.csproj
```