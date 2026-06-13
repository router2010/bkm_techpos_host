using Microsoft.EntityFrameworkCore;
using TechPosHost.Data;

namespace TechPosHost.Repository;

public class TerminalRepository
{
    private readonly string _connectionString =
        "Server=DESKTOP-K26B95Q\\SQLEXPRESS;Database=TechPosHost;Trusted_Connection=True;TrustServerCertificate=True;";

    public bool Exists(string terminalId)
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using var db = new AppDbContext(options);

        return db.Terminals.Any(x =>
            x.TerminalId == terminalId &&
            x.IsActive);
    }
}