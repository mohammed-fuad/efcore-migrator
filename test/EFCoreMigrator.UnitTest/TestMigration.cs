namespace EFCoreMigrator.UnitTest;

using System;
using System.Threading.Tasks;
using EFCoreMigrator.UnitTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class TestMigration
{
	[Fact]
	public async Task TestMigrate_EnsureMigrationsApplied()
	{
		var collection = new ServiceCollection();

		collection.AddDbContext<TestContext>(optionsBuilder =>
			optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString()));

		collection.RegisterMigrator<TestContext>();

		var serviceProvider = collection.BuildServiceProvider();

		var scope = serviceProvider.CreateScope();

		try
		{
			var context = scope.ServiceProvider.GetService<TestContext>();

			if (context is null)
			{
				throw new InvalidOperationException(
					$"Failed to migrate Context: {typeof(TestContext).FullName} because its not registered.");
			}

			var databaseMigrator = scope.ServiceProvider.GetService<IDbMigrator<TestContext>>();

			if (databaseMigrator is not null)
			{
				await databaseMigrator.MigrateAsync();
			}
		}
		catch (InvalidOperationException e)
			when (e.Message.Equals(
					"Relational-specific methods can only be used when the context is using a relational database provider."))
		{
			// Catch exception because DbProvider is InMemory.
		}
	}

	[Fact]
	public async Task TestMigrate_CatchUnRegisredMigrator()
	{
		var collection = new ServiceCollection();

		collection.RegisterMigrator<TestContext>();

		var serviceProvider = collection.BuildServiceProvider();

		var scope = serviceProvider.CreateScope();

		var context = scope.ServiceProvider.GetService<TestContext>();

		Assert.Null(context);
	}

	[Fact]
	public async Task TestMigrate_CatchUnregisteredContext()
	{
		var collection = new ServiceCollection();

		collection.AddDbContext<TestContext>(optionsBuilder =>
			optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString()));

		var serviceProvider = collection.BuildServiceProvider();

		var scope = serviceProvider.CreateScope();

		var databaseMigrator = scope.ServiceProvider.GetService<IDbMigrator<TestContext>>();

		Assert.Null(databaseMigrator);
	}
}
