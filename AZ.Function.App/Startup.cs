using AZ.Function.App.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(FunctionApp2.Startup))]
namespace FunctionApp2;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings:DefaultConnection");
        builder.Services.AddDbContext<FunctionsDbContext>(o =>
            o.UseSqlServer(connectionString));
        builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
    }
}
