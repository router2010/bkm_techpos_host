using TechPosHost.Data;

namespace TechPosHost.Repository;

public class TerminalRepository
{
    private readonly AppDbContext _db;

    public TerminalRepository(AppDbContext db)
    {
        _db = db;
    }

    public bool Exists(string terminalId)
    {
        return _db.Terminals.Any(x =>
            x.TerminalId == terminalId &&
            x.IsActive);
    }
}