# Milestone 2: OOP and JSON Persistence

**Status:** Complete

This version keeps the Milestone 1 workflow and represents customers, bicycles, and service jobs as related C# objects:

- A `Customer` has a name and a list of bicycles.
- A `Bicycle` has a model and a list of service jobs.
- A `ServiceJob` records its issue, status, estimated cost, and optional final cost.

The app can add, list, find, update, and delete service jobs. Updating a job lets the user change its status and enter a final cost. When a job is added for an existing customer and bicycle, it is attached to those existing objects. Data is loaded from and saved to `service-customers.json` in the current working directory.

New jobs start with the `Requested` status. The update menu supports `Requested`, `In Progress`, `Completed`, and `Cancelled`.

## Run Milestone 2

From the repository root, run:

```powershell
dotnet run --project BicycleServiceEvolutionPortfolio/Milestone02_OOP/BicycleService.Milestone02.OOP.csproj
```