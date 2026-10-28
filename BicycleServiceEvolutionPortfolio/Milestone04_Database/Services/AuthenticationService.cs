using System.Security.Cryptography;
using BicycleService.Milestone04.Database.Data;
using BicycleService.Milestone04.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BicycleService.Milestone04.Database.Services;

public class AuthenticationService
{
	private const int PasswordIterations = 100_000;
	private readonly BicycleServiceDbContext database;

	public UserAccount? CurrentUser { get; private set; }

	public AuthenticationService(BicycleServiceDbContext database)
	{
		this.database = database;
	}

	public string? Register(string userName, string password)
	{
		userName = userName.Trim();
		if (userName.Length < 3 || userName.Length > 30 || userName.Any(character =>
			!char.IsLetterOrDigit(character) && character != '.' && character != '-' && character != '_'))
		{
			return "Usernames must be 3-30 characters and use only letters, numbers, dots, dashes, or underscores.";
		}

		if (password.Length < 8)
		{
			return "Passwords must contain at least 8 characters.";
		}

		string normalizedUserName = userName.ToLowerInvariant();
		if (database.UserAccounts.Any(account => account.UserName.ToLower() == normalizedUserName))
		{
			return "That username is already registered.";
		}

		byte[] salt = RandomNumberGenerator.GetBytes(16);
		byte[] passwordHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, PasswordIterations, HashAlgorithmName.SHA256, 32);
		UserAccount account = new()
		{
			UserName = userName,
			PasswordSalt = Convert.ToBase64String(salt),
			PasswordHash = Convert.ToBase64String(passwordHash)
		};

		database.UserAccounts.Add(account);
		database.SaveChanges();
		CurrentUser = account;
		return null;
	}

	public bool Login(string userName, string password)
	{
		string normalizedUserName = userName.Trim().ToLowerInvariant();
		UserAccount? account = database.UserAccounts.FirstOrDefault(existingAccount =>
			existingAccount.UserName.ToLower() == normalizedUserName);

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
}