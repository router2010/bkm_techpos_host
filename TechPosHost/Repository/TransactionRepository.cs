using TechPosHost.Data;
using TechPosHost.Models;

namespace TechPosHost.Repository;

public class TransactionRepository
{
    private readonly AppDbContext _db;

    public TransactionRepository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(TransactionLog log)
    {
        _db.Transactions.Add(
            new Transaction
            {
                CreatedAt = log.CreatedAt,
                MTI = log.MTI,
                ProcessingCode = log.ProcessingCode,
                Amount = log.Amount,
                Stan = log.Stan,
                TerminalId = log.TerminalId,
                ResponseCode = log.ResponseCode,
                Rrn = log.Rrn,
                AuthCode = log.AuthCode
            });

        _db.SaveChanges();
    }

    public int Count()
    {
        return _db.Transactions.Count();
    }

    public bool Reverse(string stan)
    {
        var trx = _db.Transactions
            .OrderByDescending(x => x.Id)
            .FirstOrDefault(x =>
                x.Stan == stan &&
                !x.IsReversed);

        if (trx == null)
            return false;

        trx.IsReversed = true;

        _db.SaveChanges();

        return true;
    }
    public decimal SettlementAmount()
    {
        return _db.Transactions
            .Where(x => !x.IsReversed)
            .Sum(x => decimal.Parse(x.Amount ?? "0"));
    }
}