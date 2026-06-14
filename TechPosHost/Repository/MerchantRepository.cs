using TechPosHost.Data;

namespace TechPosHost.Repository;

public class MerchantRepository
{
    private readonly AppDbContext _db;

    public MerchantRepository(AppDbContext db)
    {
        _db = db;
    }

    public Merchant? Get(string merchantNo)
    {
        return _db.Merchants
            .FirstOrDefault(x =>
                x.MerchantNo == merchantNo &&
                x.IsActive);
    }

    public decimal SettlementAmount(string merchantNo)
    {
        var terminalIds =
            _db.Terminals
                .Where(t =>
                    t.Merchant != null &&
                    t.Merchant.MerchantNo == merchantNo)
                .Select(t => t.TerminalId)
                .ToList();

        return _db.Transactions
            .Where(x =>
                !x.IsReversed &&
                terminalIds.Contains(x.TerminalId!))
            .AsEnumerable().Sum(x => x.Amount);
    }

    public int SettlementCount(string merchantNo)
    {
        var terminalIds =
            _db.Terminals
                .Where(t =>
                    t.Merchant != null &&
                    t.Merchant.MerchantNo == merchantNo)
                .Select(t => t.TerminalId)
                .ToList();

        return _db.Transactions
            .Count(x =>
                !x.IsReversed &&
                terminalIds.Contains(x.TerminalId!));
    }

    public List<Merchant> GetAll()
    {
        return _db.Merchants
            .OrderBy(x => x.Name)
            .ToList();
    }
}