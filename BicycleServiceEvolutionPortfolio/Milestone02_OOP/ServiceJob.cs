namespace BicycleService.Milestone02.OOP;

public class ServiceJob
{
	public int Id { get; set; }
	public string Issue { get; set; } = string.Empty;
	public string Status { get; set; } = "Requested";
	public decimal EstimatedCost { get; set; }
	public decimal? FinalCost { get; set; }

	public ServiceJob()
	{
	}

	public ServiceJob(int id, string issue, decimal estimatedCost)
	{
		Id = id;
		Issue = issue;
		EstimatedCost = estimatedCost;
	}
}