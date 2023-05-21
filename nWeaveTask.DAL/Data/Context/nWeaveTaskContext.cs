using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using nWeaveTask.DAL.Data.Models;

namespace nWeaveTask.DAL;

public class nWeaveContext : IdentityDbContext<User>
{
	public nWeaveContext(DbContextOptions<nWeaveContext> options) : base(options)
	{

	}
	public DbSet<Product> Products { get; set; }
}
