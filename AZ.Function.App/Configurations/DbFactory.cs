using AZ.Function.App.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AZ.Function.App.Configurations;

public class DbFactory : IDesignTimeDbContextFactory<FunctionsDbContext>
{
    public FunctionsDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json")
                .Build();

        var builder = new DbContextOptionsBuilder<FunctionsDbContext>();

        builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new FunctionsDbContext(builder.Options);
    }
}
