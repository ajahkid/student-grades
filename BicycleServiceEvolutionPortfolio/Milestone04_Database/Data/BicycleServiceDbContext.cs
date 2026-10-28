using BicycleService.Milestone04.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace BicycleService.Milestone04.Database.Data;

public class BicycleServiceDbContext : DbContext
{
	public BicycleServiceDbContext(DbContextOptions<BicycleServiceDbContext> options) : base(options)
	{
	}

	public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
	public DbSet<Customer> Customers => Set<Customer>();
	public DbSet<Bicycle> Bicycles => Set<Bicycle>();
	public DbSet<ServiceJob> ServiceJobs => Set<ServiceJob>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserAccount>()
			.HasIndex(user => user.UserName)
			.IsUnique();

		modelBuilder.Entity<UserAccount>()
			.HasMany(user => user.Customers)
			.WithOne(customer => customer.UserAccount)
			.HasForeignKey(customer => customer.UserAccountId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Customer>()
			.HasMany(customer => customer.Bicycles)
			.WithOne(bicycle => bicycle.Customer)
			.HasForeignKey(bicycle => bicycle.CustomerId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Bicycle>()
			.HasMany(bicycle => bicycle.ServiceJobs)
			.WithOne(serviceJob => serviceJob.Bicycle)
			.HasForeignKey(serviceJob => serviceJob.BicycleId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}