List<string> serviceJobs = new List<string>();
bool isRunning = true;

while (isRunning)
{
	Console.WriteLine();
	Console.WriteLine("=== Bicycle Service ===");
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
		Console.Write("Customer name: ");
		string? customerName = Console.ReadLine();

		Console.Write("Bicycle model: ");
		string? bicycleModel = Console.ReadLine();

		Console.Write("What needs to be repaired? ");
		string? serviceIssue = Console.ReadLine();

		Console.Write("Estimated cost: ");
		string? estimatedCost = Console.ReadLine();

		string serviceJob = $"Customer: {customerName}\n" +
							$"Bicycle: {bicycleModel}\n" +
							$"Issue: {serviceIssue}\n" +
							$"Estimated cost: {estimatedCost}";

		serviceJobs.Add(serviceJob);
		Console.WriteLine("Service job added.");
	}
	else if (choice == "2" || choice == "3" || choice == "4")
	{
		Console.WriteLine("This option will be implemented in a later step.");
	}
	else
	{
		Console.WriteLine("Please enter 1, 2, 3, 4, or 0.");
	}
}
