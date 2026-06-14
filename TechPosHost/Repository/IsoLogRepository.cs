using TechPosHost.Data;

namespace TechPosHost.Repository;

public class IsoLogRepository
{
    private readonly AppDbContext _db;

    public IsoLogRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(
        string request,
        string response,
        string? mti,
        string? terminalId)
    {
        _db.IsoLogs.Add(
            new IsoLog
            {
                CreatedAt = DateTime.Now,
                Request = request,
                Response = response,
                MTI = mti,
                TerminalId = terminalId
            });

        _db.SaveChanges();
    }

    public List<IsoLog> GetAll()
    {
        return _db.IsoLogs
            .OrderByDescending(x => x.Id)
            .ToList();
    }
}