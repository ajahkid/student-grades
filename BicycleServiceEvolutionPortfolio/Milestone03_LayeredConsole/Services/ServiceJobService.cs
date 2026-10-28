using BicycleService.Milestone03.LayeredConsole.Models;
using BicycleService.Milestone03.LayeredConsole.Repositories;

namespace BicycleService.Milestone03.LayeredConsole.Services;

public class ServiceJobService
{
	private static readonly string[] validStatuses = { "Requested", "In Progress", "Completed", "Cancelled" };
	private readonly IServiceJobRepository serviceJobRepository;

	public ServiceJobService(IServiceJobRepository serviceJobRepository)
	{
		this.serviceJobRepository = serviceJobRepository;
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

		List<Customer> customers = serviceJobRepository.LoadCustomers(user.Id);
		Customer? customer = customers.FirstOrDefault(existingCustomer =>
			string.Equals(existingCustomer.Name, customerName.Trim(), StringComparison.OrdinalIgnoreCase));

		if (customer is null)
		{
			int customerId = customers.Select(existingCustomer => existingCustomer.Id).DefaultIfEmpty(0).Max() + 1;
			customer = new Customer(customerId, customerName.Trim());
			customers.Add(customer);
		}

		Bicycle? bicycle = customer.Bicycles.FirstOrDefault(existingBicycle =>
			string.Equals(existingBicycle.Model, bicycleModel.Trim(), StringComparison.OrdinalIgnoreCase));

		if (bicycle is null)
		{
			int bicycleId = customers.SelectMany(existingCustomer => existingCustomer.Bicycles).Select(existingBicycle => existingBicycle.Id).DefaultIfEmpty(0).Max() + 1;
			bicycle = new Bicycle(bicycleId, bicycleModel.Trim());
			customer.Bicycles.Add(bicycle);
		}

		int serviceJobId = customers.SelectMany(existingCustomer => existingCustomer.Bicycles).SelectMany(existingBicycle => existingBicycle.ServiceJobs).Select(existingJob => existingJob.Id).DefaultIfEmpty(0).Max() + 1;
		bicycle.ServiceJobs.Add(new ServiceJob(serviceJobId, issue.Trim(), estimatedCost));
		serviceJobRepository.SaveCustomers(user.Id, customers);
		return null;
	}

	public List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> GetAllServiceJobs(UserAccount user)
	{
		return FlattenCustomers(serviceJobRepository.LoadCustomers(user.Id));
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
		List<Customer> customers = serviceJobRepository.LoadCustomers(user.Id);
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> jobs = FlattenCustomers(customers);
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

		(Customer Customer, Bicycle Bicycle, ServiceJob Job) selectedJob = jobs[jobNumber - 1];
		selectedJob.Job.Status = validStatuses.First(validStatus => string.Equals(validStatus, status, StringComparison.OrdinalIgnoreCase));
		if (finalCost.HasValue)
		{
			selectedJob.Job.FinalCost = finalCost.Value;
		}

		serviceJobRepository.SaveCustomers(user.Id, customers);
		return null;
	}

	public string? DeleteServiceJob(UserAccount user, int jobNumber)
	{
		List<Customer> customers = serviceJobRepository.LoadCustomers(user.Id);
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> jobs = FlattenCustomers(customers);
		if (jobNumber < 1 || jobNumber > jobs.Count)
		{
			return "That is not a valid service job number.";
		}

		(Customer Customer, Bicycle Bicycle, ServiceJob Job) selectedJob = jobs[jobNumber - 1];
		selectedJob.Bicycle.ServiceJobs.Remove(selectedJob.Job);
		serviceJobRepository.SaveCustomers(user.Id, customers);
		return null;
	}

	private static List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> FlattenCustomers(List<Customer> customers)
	{
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> jobs = new();
		foreach (Customer customer in customers)
		{
			foreach (Bicycle bicycle in customer.Bicycles)
			{
				foreach (ServiceJob serviceJob in bicycle.ServiceJobs)
				{
					jobs.Add((customer, bicycle, serviceJob));
				}
			}
		}

		return jobs;
	}
}