# EFCoreMigrator

Is .net standard library as a solution to apply EF Core migrations within ASP.Net Core applications.

## Installation

It's recommended to install this package inside your API/Service project only.

```powershell
PM > Install-Package EFCoreMigrator
```

## Integrated in Asp.Net Core

Just call few extension methods in Program.cs.

```c#
using System.Reflection;
using FlexCore.Core.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Register Migrator for TestContext.
builder.Services.RegisterMigrator<TestContext>();


var app = builder.Build();

// Apply migrations for TestContext.
await app.MigrateContextAsync<TestContext>();
```
