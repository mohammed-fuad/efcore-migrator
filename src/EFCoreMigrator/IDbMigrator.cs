namespace EFCoreMigrator;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Database migrator interface.
/// </summary>
/// <typeparam name="TContext">Type of DbContext.</typeparam>
public interface IDbMigrator<in TContext>
    where TContext : DbContext
{
    /// <summary>
    /// Migrate all pending migrations.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task MigrateAsync();
}