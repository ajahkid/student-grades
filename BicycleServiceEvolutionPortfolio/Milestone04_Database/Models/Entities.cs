namespace BicycleService.Milestone04.Database.Models;

public class UserAccount
{
	public int Id { get; set; }
	public string UserName { get; set; } = string.Empty;
	public string PasswordSalt { get; set; } = string.Empty;
	public string PasswordHash { get; set; } = string.Empty;
	public List<Customer> Customers { get; set; } = new();
}

public class Customer
{
	public int Id { get; set; }
	public int UserAccountId { get; set; }
	public string Name { get; set; } = string.Empty;
	public UserAccount? UserAccount { get; set; }
	public List<Bicycle> Bicycles { get; set; } = new();
}

public class Bicycle
{
	public int Id { get; set; }
	public int CustomerId { get; set; }
	public string Model { get; set; } = string.Empty;
	public Customer? Customer { get; set; }
	public List<ServiceJob> ServiceJobs { get; set; } = new();
}

public class ServiceJob
{
	public int Id { get; set; }
	public int BicycleId { get; set; }
	public string Issue { get; set; } = string.Empty;
	public string Status { get; set; } = "Requested";
	public decimal EstimatedCost { get; set; }
	public decimal? FinalCost { get; set; }
	public Bicycle? Bicycle { get; set; }
}