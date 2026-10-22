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

void AddServiceJob()
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

void ListServiceJobs()
{
	if (serviceJobs.Count == 0)
	{
		Console.WriteLine("There are no service jobs to list.");
		return;
	}

	for (int index = 0; index < serviceJobs.Count; index++)
	{
		Console.WriteLine();
		Console.WriteLine($"Service job {index + 1}:");
		Console.WriteLine(serviceJobs[index]);
	}
}

void FindServiceJobs()
{
	if (serviceJobs.Count == 0)
	{
		Console.WriteLine("There are no service jobs to search.");
		return;
	}

	Console.Write("Enter a customer, bicycle, or issue to search for: ");
	string searchTerm = Console.ReadLine() ?? "";
	bool foundMatch = false;

	for (int index = 0; index < serviceJobs.Count; index++)
	{
		if (serviceJobs[index].Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
		{
			Console.WriteLine();
			Console.WriteLine($"Service job {index + 1}:");
			Console.WriteLine(serviceJobs[index]);
			foundMatch = true;
		}
	}

	if (!foundMatch)
	{
		Console.WriteLine("No matching service jobs found.");
	}
}

void DeleteServiceJob()
{
	if (serviceJobs.Count == 0)
	{
		Console.WriteLine("There are no service jobs to delete.");
		return;
	}

	ListServiceJobs();
	Console.Write("Enter the service job number to delete: ");
	string? jobNumberInput = Console.ReadLine();

	if (!int.TryParse(jobNumberInput, out int jobNumber) || jobNumber < 1 || jobNumber > serviceJobs.Count)
	{
		Console.WriteLine("That is not a valid service job number.");
		return;
	}

	serviceJobs.RemoveAt(jobNumber - 1);
	Console.WriteLine("Service job deleted.");
}
