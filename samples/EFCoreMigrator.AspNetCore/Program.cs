using EFCoreMigrator.AspNetCore.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TestContext>(optionsBuilder =>
	optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString()));

builder.Services.RegisterMigrator<TestContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

try
{
	await app.MigrateContextAsync<TestContext>();
}
catch (InvalidOperationException e)
	when (e.Message.Equals("Relational-specific methods can only be used when the context is using a relational database provider."))
{
	// Catch exception because DbProvider is InMemory.
}

await app.RunAsync();
