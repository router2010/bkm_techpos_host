using Microsoft.EntityFrameworkCore;

namespace TechPosHost.Data;

public class AppDbContext : DbContext
{
    public DbSet<Terminal> Terminals => Set<Terminal>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<IsoLog> IsoLogs { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<TerminalKey> TerminalKeys => Set<TerminalKey>();
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>()
            .Property(x => x.Balance)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Terminal>()
            .HasOne(x => x.Merchant)
            .WithMany()
            .HasForeignKey(x => x.MerchantId);

        modelBuilder.Entity<Transaction>()
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<TerminalKey>()
            .HasIndex(x => x.TerminalId)
            .IsUnique();

        modelBuilder.Entity<TerminalKey>()
            .Property(x => x.TerminalId)
            .HasMaxLength(50);

        modelBuilder.Entity<TerminalKey>()
            .Property(x => x.TMK)
            .HasMaxLength(200);

        modelBuilder.Entity<TerminalKey>()
            .Property(x => x.TPK)
            .HasMaxLength(200);

        modelBuilder.Entity<TerminalKey>()
            .Property(x => x.TAK)
            .HasMaxLength(200);
    }
}
