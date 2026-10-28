# Milestone 5: Basic Web API

**Status:** Complete

This version exposes bicycle service jobs through an ASP.NET Core Minimal API. It uses request/response DTOs, validation, EF Core with SQLite, and Swagger/OpenAPI.

| Method | Route | Purpose | Success response |
| --- | --- | --- | --- |
| GET | `/api/service-jobs` | List jobs | `200 OK` |
| GET | `/api/service-jobs/{id}` | Get one job | `200 OK`, or `404 Not Found` |
| POST | `/api/service-jobs` | Create a job | `201 Created`, or `400 Bad Request` |
| PUT | `/api/service-jobs/{id}` | Update a job | `200 OK`, `400 Bad Request`, or `404 Not Found` |
| DELETE | `/api/service-jobs/{id}` | Delete a job | `204 No Content`, or `404 Not Found` |

The API database is stored under the current user's Local AppData folder in `BicycleService/Milestone05/bicycle-service.db`. For testing, set `BICYCLE_SERVICE_API_DB_PATH` to use another SQLite file. The API does not include authentication; it is an educational local example.

## Run Milestone 5

From the repository root, run:

```powershell
dotnet run --project BicycleServiceEvolutionPortfolio/Milestone05_WebAPI/BicycleService.Milestone05.WebAPI.csproj
```

Open `/swagger` on the local address printed by ASP.NET Core to inspect and try the endpoints.