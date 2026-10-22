# Milestone 2: OOP and JSON Persistence

**Status:** Complete

This version keeps the Milestone 1 workflow and represents customers, bicycles, and service jobs as related C# objects:

- A `Customer` has a name and a list of bicycles.
- A `Bicycle` has a model and a list of service jobs.
- A `ServiceJob` records its issue, status, estimated cost, and optional final cost.

The app can add, list, find, and delete service jobs. When a job is added for an existing customer and bicycle, it is attached to those existing objects. Data is loaded from and saved to `service-customers.json` in the current working directory.

The menu currently creates jobs with the `Requested` status and an estimated cost. Updating a job's status or recording its final cost is not part of this version's menu.

## Run Milestone 2

From the repository root, run:

```powershell
dotnet run --project BicycleServiceEvolutionPortfolio/Milestone02_OOP/BicycleService.Milestone02.OOP.csproj
```