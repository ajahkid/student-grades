using BicycleService.Milestone03.LayeredConsole.Models;

namespace BicycleService.Milestone03.LayeredConsole.Repositories;

public interface IUserRepository
{
	List<UserAccount> LoadAll();
	void SaveAll(List<UserAccount> accounts);
}