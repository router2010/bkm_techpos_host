using TechPosHost.Data;

namespace TechPosHost.Repository;

public class TerminalKeyRepository
{
    private readonly AppDbContext _db;

    public TerminalKeyRepository(AppDbContext db)
    {
        _db = db;
    }

    public TerminalKey? Get(string terminalId)
    {
        return _db.TerminalKeys
            .FirstOrDefault(x =>
                x.TerminalId == terminalId);
    }

    public void Save(
        string terminalId,
        string tmk,
        string tpk,
        string tak)
    {
        var key = Get(terminalId);

        if (key == null)
        {
            key = new TerminalKey
            {
                TerminalId = terminalId,
                CreatedAt = DateTime.Now
            };

            _db.TerminalKeys.Add(key);
        }

        key.TMK = tmk;
        key.TPK = tpk;
        key.TAK = tak;

        _db.SaveChanges();
    }
}