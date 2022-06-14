namespace EFCoreMigrator.Internal;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

/// <summary>
/// Implementation of <see cref="IDbMigrator{TContext}"/>.
/// </summary>
/// <typeparam name="TContext">Type of DbContext.</typeparam>
internal class DbMigrator<TContext> : IDbMigrator<TContext>
    where TContext : DbContext
{
    private readonly ILogger logger;
    private readonly TContext dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbMigrator{TContext}"/> class.
    /// </summary>
    /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
    /// <param name="dbContext">The instance of <typeparamref name="TContext" />.</param>
    public DbMigrator(ILogger logger, TContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task MigrateAsync()
    {
        using (this.logger.BeginScope(
                   "Checking pending migrations for context '{ContextName}.",
                   this.dbContext.GetType().FullName))
        {
            this.logger.LogInformation("Retrieving list of pending migrations.");

            try
            {
                var migrations = (await this.dbContext.Database.GetPendingMigrationsAsync()).ToArray();

                this.logger.LogInformation("{Count} migrations are found.", migrations.Length);

                if (migrations.Any())
                {
                    this.logger.LogInformation("Applying migrations.");
                }

                await this.dbContext.Database.MigrateAsync();
            }
            catch (Exception)
            {
                this.logger.LogError("Failed to migrate context");
                throw;
            }
        }
    }
}