using BicycleService.Milestone04.Database.Data;
using BicycleService.Milestone04.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BicycleService.Milestone04.Database.Services;

public class ServiceJobService
{
	private static readonly string[] validStatuses = { "Requested", "In Progress", "Completed", "Cancelled" };
	private readonly BicycleServiceDbContext database;

	public ServiceJobService(BicycleServiceDbContext database)
	{
		this.database = database;
	}

	public string? AddServiceJob(UserAccount user, string customerName, string bicycleModel, string issue, decimal estimatedCost)
	{
		if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(bicycleModel) || string.IsNullOrWhiteSpace(issue))
		{
			return "Customer, bicycle, and issue fields are required.";
		}

		if (estimatedCost < 0)
		{
			return "Estimated cost cannot be negative.";
		}

		Customer? customer = database.Customers
			.Include(existingCustomer => existingCustomer.Bicycles)
			.FirstOrDefault(existingCustomer => existingCustomer.UserAccountId == user.Id &&
				existingCustomer.Name.ToLower() == customerName.Trim().ToLower());

		if (customer is null)
		{
			customer = new Customer { UserAccountId = user.Id, Name = customerName.Trim() };
			database.Customers.Add(customer);
		}

		Bicycle? bicycle = customer.Bicycles.FirstOrDefault(existingBicycle =>
			string.Equals(existingBicycle.Model, bicycleModel.Trim(), StringComparison.OrdinalIgnoreCase));
		if (bicycle is null)
		{
			bicycle = new Bicycle { Model = bicycleModel.Trim() };
			customer.Bicycles.Add(bicycle);
		}

		bicycle.ServiceJobs.Add(new ServiceJob { Issue = issue.Trim(), EstimatedCost = estimatedCost });
		database.SaveChanges();
		return null;
	}

	public List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> GetAllServiceJobs(UserAccount user)
	{
		List<Customer> customers = LoadUserCustomers(user.Id);
		return FlattenCustomers(customers);
	}

	public List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> FindServiceJobs(UserAccount user, string searchTerm)
	{
		return GetAllServiceJobs(user)
			.Where(job => job.Customer.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
				job.Bicycle.Model.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
				job.Job.Issue.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
			.ToList();
	}

	public string? UpdateServiceJob(UserAccount user, int jobNumber, string status, decimal? finalCost)
	{
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> jobs = FlattenCustomers(LoadUserCustomers(user.Id));
		if (jobNumber < 1 || jobNumber > jobs.Count)
		{
			return "That is not a valid service job number.";
		}

		if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
		{
			return "That is not a valid service job status.";
		}

		if (finalCost < 0)
		{
			return "Final cost cannot be negative.";
		}

		ServiceJob selectedJob = jobs[jobNumber - 1].Job;
		selectedJob.Status = validStatuses.First(validStatus => string.Equals(validStatus, status, StringComparison.OrdinalIgnoreCase));
		if (finalCost.HasValue)
		{
			selectedJob.FinalCost = finalCost.Value;
		}

		database.SaveChanges();
		return null;
	}

	public string? DeleteServiceJob(UserAccount user, int jobNumber)
	{
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> jobs = FlattenCustomers(LoadUserCustomers(user.Id));
		if (jobNumber < 1 || jobNumber > jobs.Count)
		{
			return "That is not a valid service job number.";
		}

		database.ServiceJobs.Remove(jobs[jobNumber - 1].Job);
		database.SaveChanges();
		return null;
	}

	private List<Customer> LoadUserCustomers(int userId)
	{
		return database.Customers
			.Where(customer => customer.UserAccountId == userId)
			.Include(customer => customer.Bicycles)
			.ThenInclude(bicycle => bicycle.ServiceJobs)
			.OrderBy(customer => customer.Id)
			.ToList();
	}

	private static List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> FlattenCustomers(List<Customer> customers)
	{
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> jobs = new();
		foreach (Customer customer in customers)
		{
			foreach (Bicycle bicycle in customer.Bicycles.OrderBy(bicycle => bicycle.Id))
			{
				foreach (ServiceJob job in bicycle.ServiceJobs.OrderBy(job => job.Id))
				{
					jobs.Add((customer, bicycle, job));
				}
			}
		}

		return jobs;
	}
}