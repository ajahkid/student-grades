namespace BicycleService.Milestone02.OOP;

public class Bicycle
{
	public int Id { get; set; }
	public string Model { get; set; } = string.Empty;
	public List<ServiceJob> ServiceJobs { get; set; } = new();

	public Bicycle()
	{
	}

	public Bicycle(int id, string model)
	{
		Id = id;
		Model = model;
	}
}