namespace BicycleService.Milestone05.WebAPI.Models;

public class Customer
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
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