using System.Text.Json;
using BicycleService.Milestone03.LayeredConsole.Models;

namespace BicycleService.Milestone03.LayeredConsole.Repositories;

public class JsonServiceJobRepository : IServiceJobRepository
{
	private readonly string dataDirectory;

	public JsonServiceJobRepository(string dataDirectory)
	{
		Directory.CreateDirectory(dataDirectory);
		this.dataDirectory = dataDirectory;
	}

	public List<Customer> LoadCustomers(int userId)
	{
		string filePath = GetUserDataPath(userId);
		if (!File.Exists(filePath))
		{
			return new List<Customer>();
		}

		List<Customer> customers = JsonSerializer.Deserialize<List<Customer>>(File.ReadAllText(filePath)) ?? new List<Customer>();
		foreach (Customer customer in customers)
		{
			customer.Bicycles ??= new List<Bicycle>();
			foreach (Bicycle bicycle in customer.Bicycles)
			{
				bicycle.ServiceJobs ??= new List<ServiceJob>();
			}
		}

		return customers;
	}

	public void SaveCustomers(int userId, List<Customer> customers)
	{
		string json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
		File.WriteAllText(GetUserDataPath(userId), json);
	}

	private string GetUserDataPath(int userId)
	{
		return Path.Combine(dataDirectory, $"user-{userId}-customers.json");
	}
}