using System.Globalization;
using System.Text.Json;

namespace BicycleService.Milestone02.OOP;

internal class Program
{
	private const string DataFileName = "service-customers.json";
	private static bool canSaveCustomers = true;
	private static List<Customer> customers = LoadCustomers();
	private static int nextCustomerId = customers.Select(customer => customer.Id).DefaultIfEmpty(0).Max() + 1;
	private static int nextBicycleId = customers.SelectMany(customer => customer.Bicycles).Select(bicycle => bicycle.Id).DefaultIfEmpty(0).Max() + 1;
	private static int nextServiceJobId = customers.SelectMany(customer => customer.Bicycles).SelectMany(bicycle => bicycle.ServiceJobs).Select(serviceJob => serviceJob.Id).DefaultIfEmpty(0).Max() + 1;

	private static void Main()
	{
		bool isRunning = true;

		while (isRunning)
		{
			Console.WriteLine();
			Console.WriteLine("=== Bicycle Service: Milestone 2 ===");
			Console.WriteLine("1. Add a service job");
			Console.WriteLine("2. List service jobs");
			Console.WriteLine("3. Find a service job");
			Console.WriteLine("4. Delete a service job");
			Console.WriteLine("0. Exit");
			Console.Write("Choose an option: ");

			string? choice = Console.ReadLine();

			if (choice == "0")
			{
				isRunning = false;
				Console.WriteLine("Goodbye!");
			}
			else if (choice == "1")
			{
				AddServiceJob();
			}
			else if (choice == "2")
			{
				ListServiceJobs();
			}
			else if (choice == "3")
			{
				FindServiceJobs();
			}
			else if (choice == "4")
			{
				DeleteServiceJob();
			}
			else
			{
				Console.WriteLine("Please enter 1, 2, 3, 4, or 0.");
			}
		}
	}

	private static void AddServiceJob()
	{
		string customerName = ReadRequiredText("Customer name: ");
		string bicycleModel = ReadRequiredText("Bicycle model: ");
		string issue = ReadRequiredText("What needs to be repaired? ");
		decimal estimatedCost = ReadEstimatedCost();

		Customer? customer = customers.FirstOrDefault(existingCustomer =>
			string.Equals(existingCustomer.Name, customerName, StringComparison.OrdinalIgnoreCase));

		if (customer is null)
		{
			customer = new Customer(nextCustomerId++, customerName);
			customers.Add(customer);
		}

		Bicycle? bicycle = customer.Bicycles.FirstOrDefault(existingBicycle =>
			string.Equals(existingBicycle.Model, bicycleModel, StringComparison.OrdinalIgnoreCase));

		if (bicycle is null)
		{
			bicycle = new Bicycle(nextBicycleId++, bicycleModel);
			customer.Bicycles.Add(bicycle);
		}

		bicycle.ServiceJobs.Add(new ServiceJob(nextServiceJobId++, issue, estimatedCost));
		Console.WriteLine("Service job added.");
		SaveCustomers();
	}

	private static void ListServiceJobs()
	{
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> serviceJobs = GetAllServiceJobs();

		if (serviceJobs.Count == 0)
		{
			Console.WriteLine("There are no service jobs to list.");
			return;
		}

		for (int index = 0; index < serviceJobs.Count; index++)
		{
			DisplayServiceJob(index + 1, serviceJobs[index]);
		}
	}

	private static void FindServiceJobs()
	{
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> serviceJobs = GetAllServiceJobs();

		if (serviceJobs.Count == 0)
		{
			Console.WriteLine("There are no service jobs to search.");
			return;
		}

		string searchTerm = ReadRequiredText("Enter a customer, bicycle, or issue to search for: ");
		bool foundMatch = false;

		for (int index = 0; index < serviceJobs.Count; index++)
		{
			(Customer Customer, Bicycle Bicycle, ServiceJob Job) serviceJob = serviceJobs[index];
			bool matches = serviceJob.Customer.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
				serviceJob.Bicycle.Model.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
				serviceJob.Job.Issue.Contains(searchTerm, StringComparison.OrdinalIgnoreCase);

			if (matches)
			{
				DisplayServiceJob(index + 1, serviceJob);
				foundMatch = true;
			}
		}

		if (!foundMatch)
		{
			Console.WriteLine("No matching service jobs found.");
		}
	}

	private static void DeleteServiceJob()
	{
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> serviceJobs = GetAllServiceJobs();

		if (serviceJobs.Count == 0)
		{
			Console.WriteLine("There are no service jobs to delete.");
			return;
		}

		for (int index = 0; index < serviceJobs.Count; index++)
		{
			DisplayServiceJob(index + 1, serviceJobs[index]);
		}

		Console.Write("Enter the service job number to delete: ");
		string? jobNumberInput = Console.ReadLine();

		if (!int.TryParse(jobNumberInput, out int jobNumber) || jobNumber < 1 || jobNumber > serviceJobs.Count)
		{
			Console.WriteLine("That is not a valid service job number.");
			return;
		}

		(Customer Customer, Bicycle Bicycle, ServiceJob Job) selectedJob = serviceJobs[jobNumber - 1];
		selectedJob.Bicycle.ServiceJobs.Remove(selectedJob.Job);
		Console.WriteLine("Service job deleted.");
		SaveCustomers();
	}

	private static List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> GetAllServiceJobs()
	{
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> serviceJobs = new();

		foreach (Customer customer in customers)
		{
			foreach (Bicycle bicycle in customer.Bicycles)
			{
				foreach (ServiceJob serviceJob in bicycle.ServiceJobs)
				{
					serviceJobs.Add((customer, bicycle, serviceJob));
				}
			}
		}

		return serviceJobs;
	}

	private static void DisplayServiceJob(int number, (Customer Customer, Bicycle Bicycle, ServiceJob Job) serviceJob)
	{
		Console.WriteLine();
		Console.WriteLine($"Service job {number}:");
		Console.WriteLine($"Customer: {serviceJob.Customer.Name}");
		Console.WriteLine($"Bicycle: {serviceJob.Bicycle.Model}");
		Console.WriteLine($"Issue: {serviceJob.Job.Issue}");
		Console.WriteLine($"Status: {serviceJob.Job.Status}");
		Console.WriteLine($"Estimated cost: {serviceJob.Job.EstimatedCost.ToString("C", CultureInfo.CurrentCulture)}");
		if (serviceJob.Job.FinalCost.HasValue)
		{
			Console.WriteLine($"Final cost: {serviceJob.Job.FinalCost.Value.ToString("C", CultureInfo.CurrentCulture)}");
		}
	}

	private static string ReadRequiredText(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);
			string? input = Console.ReadLine();

			if (!string.IsNullOrWhiteSpace(input))
			{
				return input.Trim();
			}

			Console.WriteLine("Please enter a value.");
		}
	}

	private static decimal ReadEstimatedCost()
	{
		while (true)
		{
			Console.Write("Estimated cost: ");
			string? input = Console.ReadLine();

			if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal estimatedCost) && estimatedCost >= 0)
			{
				return estimatedCost;
			}

			Console.WriteLine("Please enter a valid non-negative cost.");
		}
	}

	private static List<Customer> LoadCustomers()
	{
		if (!File.Exists(DataFileName))
		{
			return new List<Customer>();
		}

		try
		{
			List<Customer> loadedCustomers = JsonSerializer.Deserialize<List<Customer>>(File.ReadAllText(DataFileName)) ?? new List<Customer>();

			foreach (Customer customer in loadedCustomers)
			{
				customer.Bicycles ??= new List<Bicycle>();
				foreach (Bicycle bicycle in customer.Bicycles)
				{
					bicycle.ServiceJobs ??= new List<ServiceJob>();
				}
			}

			return loadedCustomers;
		}
		catch (JsonException)
		{
			canSaveCustomers = false;
			Console.WriteLine($"Could not read {DataFileName}; existing data will not be overwritten.");
			return new List<Customer>();
		}
		catch (Exception exception) when (exception is IOException || exception is UnauthorizedAccessException)
		{
			canSaveCustomers = false;
			Console.WriteLine($"Could not read {DataFileName}: {exception.Message}");
			return new List<Customer>();
		}
	}

	private static void SaveCustomers()
	{
		if (!canSaveCustomers)
		{
			Console.WriteLine("Saving is disabled until the existing JSON file can be read.");
			return;
		}

		try
		{
			string json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(DataFileName, json);
			Console.WriteLine($"Service jobs saved to {DataFileName}.");
		}
		catch (Exception exception) when (exception is IOException || exception is UnauthorizedAccessException)
		{
			Console.WriteLine($"Could not save service jobs: {exception.Message}");
		}
	}
}