using System.Text.Json;

const string serviceJobsFile = "service-jobs.json";
bool canSaveServiceJobs = true;
List<string> serviceJobs = LoadServiceJobs();
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
	SaveServiceJobs();
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
	SaveServiceJobs();
}

List<string> LoadServiceJobs()
{
	if (!File.Exists(serviceJobsFile))
	{
		return new List<string>();
	}

	try
	{
		return JsonSerializer.Deserialize<List<string>>(File.ReadAllText(serviceJobsFile)) ?? new List<string>();
	}
	catch (JsonException)
	{
		canSaveServiceJobs = false;
		Console.WriteLine($"Could not read {serviceJobsFile}; existing data will not be overwritten.");
		return new List<string>();
	}
}

void SaveServiceJobs()
{
	if (!canSaveServiceJobs)
	{
		Console.WriteLine("Saving is disabled until the existing JSON file is fixed.");
		return;
	}

	try
	{
		string json = JsonSerializer.Serialize(serviceJobs, new JsonSerializerOptions { WriteIndented = true });
		File.WriteAllText(serviceJobsFile, json);
		Console.WriteLine($"Service jobs saved to {serviceJobsFile}.");
	}
	catch (Exception exception) when (exception is IOException || exception is UnauthorizedAccessException)
	{
		Console.WriteLine($"Could not save service jobs: {exception.Message}");
	}
}
