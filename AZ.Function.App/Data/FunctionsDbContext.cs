using AZ.Function.App.Models;
using Microsoft.EntityFrameworkCore;

namespace AZ.Function.App.Data;

public class FunctionsDbContext : DbContext
{
	public DbSet<Cliente> Clientes { get; set; }

	public FunctionsDbContext(DbContextOptions<FunctionsDbContext> options) : base(options)
	{
		Clientes = Set<Cliente>();
	}
}