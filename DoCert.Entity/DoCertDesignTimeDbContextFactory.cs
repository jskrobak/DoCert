using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DoCert.Entity;

public class DoCertDesignTimeDbContextFactory:IDesignTimeDbContextFactory<DoCertDbContext>
{
    public DoCertDbContext CreateDbContext(string[] args)
    {
        // Commands EF Core Migrations (Add-Migration, ...) tooling get DbContext from this method. (+ CodeGenerator)
        // InMemory provider cannot be used for EF Core Migrations tooling, SqlServer provider has to be used.
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        // Current path is for CodeGenerator DataLayer
        // We need to read Entity configuration, Entity\bin\Debug(Release)\nestandard2.0.
        var configuration = new ConfigurationBuilder()
            .SetBasePath(System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location))
            .AddJsonFile("appSettings.Entity.json")
            .AddJsonFile($"appSettings.Entity.{environment}.json", true)
            .AddJsonFile($"appSettings.Entity.{environment}.local.json", true) // .gitignored
            .Build();

        var connectionString = configuration.GetConnectionString("Database");

        //Console.WriteLine(connectionString);
        return new DoCertDbContext(new DbContextOptionsBuilder<DoCertDbContext>().UseSqlite(connectionString).Options);
    }
}