using System.Globalization;
using BicycleService.Milestone04.Database.Models;
using BicycleService.Milestone04.Database.Services;

namespace BicycleService.Milestone04.Database.Presentation;

public class ConsoleApplication
{
	private readonly AuthenticationService authenticationService;
	private readonly ServiceJobService serviceJobService;

	public ConsoleApplication(AuthenticationService authenticationService, ServiceJobService serviceJobService)
	{
		this.authenticationService = authenticationService;
		this.serviceJobService = serviceJobService;
	}

	public void Run()
	{
		bool isRunning = true;
		while (isRunning)
		{
			isRunning = authenticationService.CurrentUser is null
				? ShowAccountMenu()
				: ShowServiceMenu(authenticationService.CurrentUser);
		}
	}

	private bool ShowAccountMenu()
	{
		Console.WriteLine();
		Console.WriteLine("=== Bicycle Service: Database Account ===");
		Console.WriteLine("1. Register");
		Console.WriteLine("2. Log in");
		Console.WriteLine("0. Exit");
		Console.Write("Choose an option: ");

		switch (Console.ReadLine())
		{
			case "1":
				Register();
				return true;
			case "2":
				LogIn();
				return true;
			case "0":
				Console.WriteLine("Goodbye!");
				return false;
			default:
				Console.WriteLine("Please enter 1, 2, or 0.");
				return true;
		}
	}

	private bool ShowServiceMenu(UserAccount user)
	{
		Console.WriteLine();
		Console.WriteLine($"=== Bicycle Service Database: {user.UserName} ===");
		Console.WriteLine("1. Add a service job");
		Console.WriteLine("2. List service jobs");
		Console.WriteLine("3. Find a service job");
		Console.WriteLine("4. Update a service job");
		Console.WriteLine("5. Delete a service job");
		Console.WriteLine("6. Log out");
		Console.WriteLine("0. Exit");
		Console.Write("Choose an option: ");

		switch (Console.ReadLine())
		{
			case "1":
				AddServiceJob(user);
				return true;
			case "2":
				ListServiceJobs(user);
				return true;
			case "3":
				FindServiceJobs(user);
				return true;
			case "4":
				UpdateServiceJob(user);
				return true;
			case "5":
				DeleteServiceJob(user);
				return true;
			case "6":
				authenticationService.Logout();
				Console.WriteLine("You have been logged out.");
				return true;
			case "0":
				Console.WriteLine("Goodbye!");
				return false;
			default:
				Console.WriteLine("Please enter a number from 0 to 6.");
				return true;
		}
	}

	private void Register()
	{
		string userName = ReadRequiredText("Choose a username: ");
		string password = ReadRequiredText("Choose a password (at least 8 characters): ");
		string? error = authenticationService.Register(userName, password);
		Console.WriteLine(error ?? "Account created and logged in.");
	}

	private void LogIn()
	{
		string userName = ReadRequiredText("Username: ");
		string password = ReadRequiredText("Password: ");
		if (authenticationService.Login(userName, password))
		{
			Console.WriteLine($"Welcome, {authenticationService.CurrentUser!.UserName}.");
		}
		else
		{
			Console.WriteLine("The username or password is incorrect.");
		}
	}

	private void AddServiceJob(UserAccount user)
	{
		string customerName = ReadRequiredText("Customer name: ");
		string bicycleModel = ReadRequiredText("Bicycle model: ");
		string issue = ReadRequiredText("What needs to be repaired? ");
		decimal estimatedCost = ReadCost("Estimated cost: ");
		string? error = serviceJobService.AddServiceJob(user, customerName, bicycleModel, issue, estimatedCost);
		Console.WriteLine(error ?? "Service job saved to the database.");
	}

	private void ListServiceJobs(UserAccount user)
	{
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> jobs = serviceJobService.GetAllServiceJobs(user);
		if (jobs.Count == 0)
		{
			Console.WriteLine("There are no service jobs for this account.");
			return;
		}

		for (int index = 0; index < jobs.Count; index++)
		{
			DisplayServiceJob(index + 1, jobs[index]);
		}
	}

	private void FindServiceJobs(UserAccount user)
	{
		string searchTerm = ReadRequiredText("Enter a customer, bicycle, or issue to search for: ");
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> matches = serviceJobService.FindServiceJobs(user, searchTerm);
		if (matches.Count == 0)
		{
			Console.WriteLine("No matching service jobs found.");
			return;
		}

		for (int index = 0; index < matches.Count; index++)
		{
			DisplayServiceJob(index + 1, matches[index]);
		}
	}

	private void UpdateServiceJob(UserAccount user)
	{
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> jobs = serviceJobService.GetAllServiceJobs(user);
		if (jobs.Count == 0)
		{
			Console.WriteLine("There are no service jobs to update.");
			return;
		}

		for (int index = 0; index < jobs.Count; index++)
		{
			DisplayServiceJob(index + 1, jobs[index]);
		}

		int jobNumber = ReadPositiveNumber("Enter the service job number to update: ");
		Console.WriteLine("1. Requested");
		Console.WriteLine("2. In Progress");
		Console.WriteLine("3. Completed");
		Console.WriteLine("4. Cancelled");
		string? status = Console.ReadLine() switch
		{
			"1" => "Requested",
			"2" => "In Progress",
			"3" => "Completed",
			"4" => "Cancelled",
			_ => null
		};

		if (status is null)
		{
			Console.WriteLine("That is not a valid status.");
			return;
		}

		decimal? finalCost = ReadOptionalCost("Final cost (press Enter to leave unchanged): ");
		string? error = serviceJobService.UpdateServiceJob(user, jobNumber, status, finalCost);
		Console.WriteLine(error ?? "Service job updated.");
	}

	private void DeleteServiceJob(UserAccount user)
	{
		List<(Customer Customer, Bicycle Bicycle, ServiceJob Job)> jobs = serviceJobService.GetAllServiceJobs(user);
		if (jobs.Count == 0)
		{
			Console.WriteLine("There are no service jobs to delete.");
			return;
		}

		for (int index = 0; index < jobs.Count; index++)
		{
			DisplayServiceJob(index + 1, jobs[index]);
		}

		int jobNumber = ReadPositiveNumber("Enter the service job number to delete: ");
		string? error = serviceJobService.DeleteServiceJob(user, jobNumber);
		Console.WriteLine(error ?? "Service job deleted.");
	}

	private static void DisplayServiceJob(int number, (Customer Customer, Bicycle Bicycle, ServiceJob Job) serviceJob)
	{
		Console.WriteLine();
		Console.WriteLine($"Service job {number}:");
		Console.WriteLine($"Customer: {serviceJob.Customer.Name}");
		Console.WriteLine($"Bicycle: {serviceJob.Bicycle.Model}");
		Console.WriteLine($"Issue: {serviceJob.Job.Issue}");
		Console.WriteLine($"Status: {serviceJob.Job.Status}");
		Console.WriteLine($"Estimated cost: {serviceJob.Job.EstimatedCost.ToString("C", CultureInfo.CurrentCulture)}");
		if (serviceJob.Job.FinalCost.HasValue)
		{
			Console.WriteLine($"Final cost: {serviceJob.Job.FinalCost.Value.ToString("C", CultureInfo.CurrentCulture)}");
		}
	}

	private static string ReadRequiredText(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);
			string? input = Console.ReadLine();
			if (!string.IsNullOrWhiteSpace(input))
			{
				return input.Trim();
			}

			Console.WriteLine("Please enter a value.");
		}
	}

	private static decimal ReadCost(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);
			string? input = Console.ReadLine();
			if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal cost) && cost >= 0)
			{
				return cost;
			}

			Console.WriteLine("Enter a valid non-negative amount.");
		}
	}

	private static decimal? ReadOptionalCost(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);
			string? input = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(input))
			{
				return null;
			}

			if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal cost) && cost >= 0)
			{
				return cost;
			}

			Console.WriteLine("Enter a valid non-negative amount, or press Enter to leave it unchanged.");
		}
	}

	private static int ReadPositiveNumber(string prompt)
	{
		while (true)
		{
			Console.Write(prompt);
			if (int.TryParse(Console.ReadLine(), out int number) && number > 0)
			{
				return number;
			}

			Console.WriteLine("Enter a positive number.");
		}
	}
}