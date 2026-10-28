# Bicycle Service Project Evolution Portfolio

**Assignment topic:** Bicycle Service (#20)

## Project Idea

A simple program for a bicycle repair shop. Staff can record a customer's bicycle and the repair work requested for it.

The same idea will grow through each milestone:

- A **customer** may have one or more bicycles.
- A **bicycle** may have several service jobs over time.
- A **service job** records the problem, the work requested, its status, and its estimated or final cost.

## Current Progress

Milestone 1 is complete. Its console menu supports adding, listing, finding, and deleting service jobs, and saves them to `service-jobs.json` in the current working directory.

Milestone 2 is complete. A separate console app models customers, bicycles, and service jobs as related objects and saves the nested data to `service-customers.json`. The same customer and bicycle are reused when another job is added for them. Service jobs can also be updated with a status and final cost.

Milestone 3 is complete. It separates presentation, service logic, and JSON repositories. Users can register, log in, and log out, and each account has its own service-job data file.

Milestone 4 is complete. A separate EF Core app stores user accounts, customers, bicycles, and service jobs in related SQLite tables.

Milestone 5 is planned only. Its folder contains notes, not a working application yet.

## Milestone Plan

| Milestone | What the version will add | Status |
| --- | --- | --- |
| 1. Basic Console | Menu, variables, if/else, loop, methods, collection, Add/List/Find/Delete, JSON file | Complete |
| 2. OOP and Persistence | `Customer`, `Bicycle`, and `ServiceJob` classes; properties; constructors; encapsulation; JSON file | Complete |
| 3. Layered Multi-user Console | Presentation, service, and repository layers; validation; register/login/logout; each user sees their own records | Complete |
| 4. Database | SQL tables and relationships; EF Core with SQLite; basic database queries | Complete |
| 5. Web API | ASP.NET Core endpoints; GET, POST, PUT, DELETE; DTOs; validation; HTTP status codes; Swagger | Planned |

## Run Milestone 1

From the repository root, run:

```powershell
dotnet run --project BicycleServiceEvolutionPortfolio/Milestone01_BasicConsole/BicycleService.Milestone01.BasicConsole.csproj
```

## Run Milestone 2

From the repository root, run:

```powershell
dotnet run --project BicycleServiceEvolutionPortfolio/Milestone02_OOP/BicycleService.Milestone02.OOP.csproj
```

## Run Milestone 3

From the repository root, run:

```powershell
dotnet run --project BicycleServiceEvolutionPortfolio/Milestone03_LayeredConsole/BicycleService.Milestone03.LayeredConsole.csproj
```

## Run Milestone 4

From the repository root, run:

```powershell
dotnet run --project BicycleServiceEvolutionPortfolio/Milestone04_Database/BicycleService.Milestone04.Database.csproj
```

## Understanding the Code

The code is intentionally written with basic C# features and clear names. The [milestone report](MILESTONE_REPORT.md) records what changed at each checkpoint and why. Read the source files in each milestone folder to compare the versions.

## GitHub Progress

Important checkpoints will be committed and pushed on the Bicycle Service portfolio branch. Git history shows when the project changed; these folders keep each required version available to inspect and run.
