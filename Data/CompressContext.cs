using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



public class CompressContext : IdentityDbContext<IdentityUser>
{
    // when using mysql connection it is enough to have two methods
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    public CompressContext(DbContextOptions<CompressContext> options) : base(options)
    {
    }


    public DbSet<Customer> Customers { get; set; }
    // Define other DbSet properties for your entities
}
