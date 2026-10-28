using BicycleService.Milestone03.LayeredConsole.Models;

namespace BicycleService.Milestone03.LayeredConsole.Repositories;

public interface IServiceJobRepository
{
	List<Customer> LoadCustomers(int userId);
	void SaveCustomers(int userId, List<Customer> customers);
}