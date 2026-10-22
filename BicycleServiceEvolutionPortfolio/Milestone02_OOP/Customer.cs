namespace BicycleService.Milestone02.OOP;

public class Customer
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public List<Bicycle> Bicycles { get; set; } = new();

	public Customer()
	{
	}

	public Customer(int id, string name)
	{
		Id = id;
		Name = name;
	}
}