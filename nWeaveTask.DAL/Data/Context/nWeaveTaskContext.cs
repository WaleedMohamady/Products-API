using Microsoft.EntityFrameworkCore;

namespace nWeaveTask.DAL;

public class nWeaveContext : DbContext
{
	public nWeaveContext(DbContextOptions<nWeaveContext> options) : base(options)
	{

	}
	public DbSet<Product> Products { get; set; }
}
//Waleed
//Waleed
//Waleed