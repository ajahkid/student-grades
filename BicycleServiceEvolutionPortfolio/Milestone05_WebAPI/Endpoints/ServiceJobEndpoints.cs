using System.ComponentModel.DataAnnotations;
using BicycleService.Milestone05.WebAPI.Data;
using BicycleService.Milestone05.WebAPI.Dtos;
using BicycleService.Milestone05.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BicycleService.Milestone05.WebAPI.Endpoints;

public static class ServiceJobEndpoints
{
	public static IEndpointRouteBuilder MapServiceJobEndpoints(this IEndpointRouteBuilder endpoints)
	{
		RouteGroupBuilder serviceJobs = endpoints.MapGroup("/api/service-jobs").WithTags("Service Jobs");

		serviceJobs.MapGet("/", async (BicycleServiceDbContext database) =>
		{
			List<ServiceJob> jobs = await database.ServiceJobs
				.AsNoTracking()
				.Include(job => job.Bicycle)
				.ThenInclude(bicycle => bicycle!.Customer)
				.OrderBy(job => job.Id)
				.ToListAsync();

			return Results.Ok(jobs.Select(ToResponse).ToList());
		})
			.WithName("GetServiceJobs")
			.Produces<List<ServiceJobResponse>>(StatusCodes.Status200OK);

		serviceJobs.MapGet("/{id:int}", async (int id, BicycleServiceDbContext database) =>
		{
			ServiceJob? job = await database.ServiceJobs
				.AsNoTracking()
				.Include(existingJob => existingJob.Bicycle)
				.ThenInclude(bicycle => bicycle!.Customer)
				.FirstOrDefaultAsync(existingJob => existingJob.Id == id);

			return job is null ? Results.NotFound() : Results.Ok(ToResponse(job));
		})
			.WithName("GetServiceJob")
			.Produces<ServiceJobResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound);

		serviceJobs.MapPost("/", async (CreateServiceJobRequest request, BicycleServiceDbContext database) =>
		{
			Dictionary<string, string[]> errors = Validate(request);
			if (errors.Count > 0)
			{
				return Results.ValidationProblem(errors);
			}

			Customer? customer = await database.Customers
				.Include(existingCustomer => existingCustomer.Bicycles)
				.FirstOrDefaultAsync(existingCustomer => existingCustomer.Name.ToLower() == request.CustomerName.Trim().ToLower());

			if (customer is null)
			{
				customer = new Customer { Name = request.CustomerName.Trim() };
				database.Customers.Add(customer);
			}

			Bicycle? bicycle = customer.Bicycles.FirstOrDefault(existingBicycle =>
				string.Equals(existingBicycle.Model, request.BicycleModel.Trim(), StringComparison.OrdinalIgnoreCase));
			if (bicycle is null)
			{
				bicycle = new Bicycle { Model = request.BicycleModel.Trim() };
				customer.Bicycles.Add(bicycle);
			}

			bicycle.ServiceJobs.Add(new ServiceJob
			{
				Issue = request.Issue.Trim(),
				EstimatedCost = request.EstimatedCost
			});
			await database.SaveChangesAsync();

			ServiceJob createdJob = bicycle.ServiceJobs[^1];
			return Results.Created($"/api/service-jobs/{createdJob.Id}", ToResponse(createdJob, customer, bicycle));
		})
			.WithName("CreateServiceJob")
			.Accepts<CreateServiceJobRequest>("application/json")
			.Produces<ServiceJobResponse>(StatusCodes.Status201Created)
			.ProducesValidationProblem();

		serviceJobs.MapPut("/{id:int}", async (int id, UpdateServiceJobRequest request, BicycleServiceDbContext database) =>
		{
			Dictionary<string, string[]> errors = Validate(request);
			if (errors.Count > 0)
			{
				return Results.ValidationProblem(errors);
			}

			ServiceJob? job = await database.ServiceJobs
				.Include(existingJob => existingJob.Bicycle)
				.ThenInclude(bicycle => bicycle!.Customer)
				.FirstOrDefaultAsync(existingJob => existingJob.Id == id);
			if (job is null)
			{
				return Results.NotFound();
			}

			job.Issue = request.Issue.Trim();
			job.Status = request.Status;
			job.EstimatedCost = request.EstimatedCost;
			job.FinalCost = request.FinalCost;
			await database.SaveChangesAsync();

			return Results.Ok(ToResponse(job));
		})
			.WithName("UpdateServiceJob")
			.Accepts<UpdateServiceJobRequest>("application/json")
			.Produces<ServiceJobResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status404NotFound)
			.ProducesValidationProblem();

		serviceJobs.MapDelete("/{id:int}", async (int id, BicycleServiceDbContext database) =>
		{
			ServiceJob? job = await database.ServiceJobs.FindAsync(id);
			if (job is null)
			{
				return Results.NotFound();
			}

			database.ServiceJobs.Remove(job);
			await database.SaveChangesAsync();
			return Results.NoContent();
		})
			.WithName("DeleteServiceJob")
			.Produces(StatusCodes.Status204NoContent)
			.Produces(StatusCodes.Status404NotFound);

		return endpoints;
	}

	private static Dictionary<string, string[]> Validate(object request)
	{
		List<ValidationResult> results = new();
		ValidationContext context = new(request);
		Validator.TryValidateObject(request, context, results, validateAllProperties: true);

		return results
			.SelectMany(result => result.MemberNames.DefaultIfEmpty(string.Empty)
				.Select(memberName => new { MemberName = memberName, ErrorMessage = result.ErrorMessage ?? "Invalid value." }))
			.GroupBy(result => result.MemberName)
			.ToDictionary(group => group.Key, group => group.Select(result => result.ErrorMessage).ToArray());
	}

	private static ServiceJobResponse ToResponse(ServiceJob job)
	{
		return ToResponse(job, job.Bicycle?.Customer, job.Bicycle);
	}

	private static ServiceJobResponse ToResponse(ServiceJob job, Customer? customer, Bicycle? bicycle)
	{
		return new ServiceJobResponse
		{
			Id = job.Id,
			CustomerName = customer?.Name ?? string.Empty,
			BicycleModel = bicycle?.Model ?? string.Empty,
			Issue = job.Issue,
			Status = job.Status,
			EstimatedCost = job.EstimatedCost,
			FinalCost = job.FinalCost
		};
	}
}