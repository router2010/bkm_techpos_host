using Microsoft.EntityFrameworkCore;

namespace TechPosHost.Data;

public class AppDbContext : DbContext
{
    public DbSet<Terminal> Terminals => Set<Terminal>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
