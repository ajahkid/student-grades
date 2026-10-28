namespace BicycleService.Milestone03.LayeredConsole.Models;

public class Customer
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public List<Bicycle> Bicycles { get; set; } = new();

	public Customer() { }

	public Customer(int id, string name)
	{
		Id = id;
		Name = name;
	}
}

public class Bicycle
{
	public int Id { get; set; }
	public string Model { get; set; } = string.Empty;
	public List<ServiceJob> ServiceJobs { get; set; } = new();

	public Bicycle() { }

	public Bicycle(int id, string model)
	{
		Id = id;
		Model = model;
	}
}

public class ServiceJob
{
	public int Id { get; set; }
	public string Issue { get; set; } = string.Empty;
	public string Status { get; set; } = "Requested";
	public decimal EstimatedCost { get; set; }
	public decimal? FinalCost { get; set; }

	public ServiceJob() { }

	public ServiceJob(int id, string issue, decimal estimatedCost)
	{
		Id = id;
		Issue = issue;
		EstimatedCost = estimatedCost;
	}
}

public class UserAccount
{
	public int Id { get; set; }
	public string UserName { get; set; } = string.Empty;
	public string PasswordSalt { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
}