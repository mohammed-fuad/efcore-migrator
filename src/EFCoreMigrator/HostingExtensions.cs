// ReSharper disable once CheckNamespace; Justification: Extension methods for a type in this namespace.

namespace Microsoft.Extensions.Hosting;

using System.Reflection;
using EFCoreMigrator;
using EFCoreMigrator.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Hosting extensions.
/// </summary>
public static class HostingExtensions
{
	/// <summary>
	/// Migrate migrations.
	/// </summary>
	/// <typeparam name="TContext">Type of <see cref="DbContext"/>.</typeparam>
	/// <param name="host">An instance of an <see cref="IHost"/>.</param>
	/// <exception cref="InvalidOperationException">When context is not registered.</exception>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	public static async Task MigrateContextAsync<TContext>(this IHost host)
		where TContext : DbContext
	{
		using var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

		var context = scope.ServiceProvider.GetService(typeof(TContext));

		if (context is null)
		{
			throw new InvalidOperationException(
				$"Failed to migrate Context: {typeof(TContext).FullName} because its not registered.");
		}

		var databaseMigrator = scope.ServiceProvider.GetService<IDbMigrator<TContext>>();

		if (databaseMigrator is not null)
		{
			await databaseMigrator.MigrateAsync();
		}
	}

	/// <summary>
	/// Register migration of given context.
	/// </summary>
	/// <param name="collection">A <see cref="IServiceCollection"/>.</param>
	/// <typeparam name="TContext">Type of <see cref="DbContext"/>.</typeparam>
	/// <returns>Modified <see cref="IServiceCollection"/>.</returns>
	public static IServiceCollection RegisterMigrator<TContext>(this IServiceCollection collection)
		where TContext : DbContext
	{
		collection.AddTransient<IDbMigrator<TContext>, DbMigrator<TContext>>();
		collection.AddLogging();

		return collection;
	}
}
