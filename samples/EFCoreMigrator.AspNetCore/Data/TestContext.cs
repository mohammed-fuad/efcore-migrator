namespace EFCoreMigrator.AspNetCore.Data;

using Microsoft.EntityFrameworkCore;

public class TestContext : DbContext
{
	public TestContext(DbContextOptions<TestContext> options)
		: base(options)
	{
	}

	public virtual DbSet<CarModel> Cars { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<CarModel>()
			.HasKey(c => c.Id);
	}
}
