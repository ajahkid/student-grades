using System.Text.Json;
using BicycleService.Milestone03.LayeredConsole.Models;

namespace BicycleService.Milestone03.LayeredConsole.Repositories;

public class JsonUserRepository : IUserRepository
{
	private readonly string usersFilePath;

	public JsonUserRepository(string dataDirectory)
	{
		Directory.CreateDirectory(dataDirectory);
		usersFilePath = Path.Combine(dataDirectory, "users.json");
	}

	public List<UserAccount> LoadAll()
	{
		if (!File.Exists(usersFilePath))
		{
			return new List<UserAccount>();
		}

		return JsonSerializer.Deserialize<List<UserAccount>>(File.ReadAllText(usersFilePath)) ?? new List<UserAccount>();
	}

	public void SaveAll(List<UserAccount> accounts)
	{
		string json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
		File.WriteAllText(usersFilePath, json);
	}
}