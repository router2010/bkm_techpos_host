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
    public List<Terminal> GetAll()
    {
        return _db.Terminals
            .OrderBy(x => x.TerminalId)
            .ToList();
    }

    public Terminal? Get(string terminalId)
    {
        return _db.Terminals
            .FirstOrDefault(x => x.TerminalId == terminalId);
    }

    public void Add(Terminal terminal)
    {
        _db.Terminals.Add(terminal);
        _db.SaveChanges();
    }

    public bool SetActive(string terminalId, bool active)
    {
        var terminal = _db.Terminals
            .FirstOrDefault(x => x.TerminalId == terminalId);

        if (terminal == null)
            return false;

        terminal.IsActive = active;

        _db.SaveChanges();

        return true;
    }
}