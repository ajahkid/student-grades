using System.Security.Cryptography;
using BicycleService.Milestone03.LayeredConsole.Models;
using BicycleService.Milestone03.LayeredConsole.Repositories;

namespace BicycleService.Milestone03.LayeredConsole.Services;

public class AuthenticationService
{
	private const int PasswordIterations = 100_000;
	private readonly IUserRepository userRepository;
	private readonly List<UserAccount> accounts;

	public UserAccount? CurrentUser { get; private set; }

	public AuthenticationService(IUserRepository userRepository)
	{
		this.userRepository = userRepository;
		accounts = userRepository.LoadAll();
	}

	public string? Register(string userName, string password)
	{
		userName = userName.Trim();	
		if (!IsValidUserName(userName))
		{
			return "Usernames must be 3-30 characters and use only letters, numbers, dots, dashes, or underscores.";
		}

		if (password.Length < 8)
		{
			return "Passwords must contain at least 8 characters.";
		}

		if (accounts.Any(account => string.Equals(account.UserName, userName, StringComparison.OrdinalIgnoreCase)))
		{
			return "That username is already registered.";
		}

		byte[] salt = RandomNumberGenerator.GetBytes(16);
		byte[] passwordHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, PasswordIterations, HashAlgorithmName.SHA256, 32);
		int userId = accounts.Select(account => account.Id).DefaultIfEmpty(0).Max() + 1;
		UserAccount account = new()
		{
			Id = userId,
			UserName = userName,
			PasswordSalt = Convert.ToBase64String(salt),
			PasswordHash = Convert.ToBase64String(passwordHash)
		};

		accounts.Add(account);
		userRepository.SaveAll(accounts);
		CurrentUser = account;
		return null;
	}

	public bool Login(string userName, string password)
	{
		UserAccount? account = accounts.FirstOrDefault(existingAccount =>
			string.Equals(existingAccount.UserName, userName.Trim(), StringComparison.OrdinalIgnoreCase));

		if (account is null)
		{
			return false;
		}

		try
		{
			byte[] salt = Convert.FromBase64String(account.PasswordSalt);
			byte[] expectedHash = Convert.FromBase64String(account.PasswordHash);
			byte[] providedHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, PasswordIterations, HashAlgorithmName.SHA256, expectedHash.Length);

			if (!CryptographicOperations.FixedTimeEquals(providedHash, expectedHash))
			{
				return false;
			}
		}
		catch (FormatException)
		{
			return false;
		}

		CurrentUser = account;
		return true;
	}

	public void Logout()
	{
		CurrentUser = null;
	}

	private static bool IsValidUserName(string userName)
	{
		if (userName.Length < 3 || userName.Length > 30)
		{
			return false;
		}

		foreach (char character in userName)
		{
			if (!char.IsLetterOrDigit(character) && character != '.' && character != '-' && character != '_')
			{
				return false;
			}
		}

		return true;
	}
}