using TechPosHost.Data;
using TechPosHost.Dto;
using TechPosHost.Models;

namespace TechPosHost.Repository;

public class TransactionRepository
{
    private readonly AppDbContext _db;

    public TransactionRepository(AppDbContext db)
    {
        _db = db;
    }
    public DailyReportDto DailyReport()
    {
        return new DailyReportDto
        {
            SaleCount =
                _db.Transactions.Count(x =>
                    x.TransactionType == "SALE" &&
                    !x.IsVoided),

            SaleAmount =
                _db.Transactions
                    .Where(x =>
                        x.TransactionType == "SALE" &&
                        !x.IsVoided)
                    .Sum(x => x.Amount),

            RefundCount =
                _db.Transactions.Count(x =>
                    x.TransactionType == "REFUND"),

            RefundAmount =
                _db.Transactions
                    .Where(x =>
                        x.TransactionType == "REFUND")
                    .Sum(x => x.Amount),

            VoidCount =
                _db.Transactions.Count(x =>
                    x.IsVoided),

            VoidAmount =
                _db.Transactions
                    .Where(x =>
                        x.IsVoided)
                    .Sum(x => x.Amount)
        };
    }
    public TerminalReportDto TerminalReport(
    string terminalId)
    {
        return new TerminalReportDto
        {
            TerminalId = terminalId,

            SaleCount =
                _db.Transactions.Count(x =>
                    x.TerminalId == terminalId &&
                    x.TransactionType == "SALE" &&
                    !x.IsVoided),

            SaleAmount =
                _db.Transactions
                    .Where(x =>
                        x.TerminalId == terminalId &&
                        x.TransactionType == "SALE" &&
                        !x.IsVoided)
                    .Sum(x => x.Amount),

            RefundCount =
                _db.Transactions.Count(x =>
                    x.TerminalId == terminalId &&
                    x.TransactionType == "REFUND"),

            RefundAmount =
                _db.Transactions
                    .Where(x =>
                        x.TerminalId == terminalId &&
                        x.TransactionType == "REFUND")
                    .Sum(x => x.Amount),

            VoidCount =
                _db.Transactions.Count(x =>
                    x.TerminalId == terminalId &&
                    x.IsVoided),

            VoidAmount =
                _db.Transactions
                    .Where(x =>
                        x.TerminalId == terminalId &&
                        x.IsVoided)
                    .Sum(x => x.Amount)
        };
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
                Pan = log.Pan,
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

    public Transaction? Reverse(string stan)
    {
        var trx = _db.Transactions
            .OrderByDescending(x => x.Id)
            .FirstOrDefault(x =>
                x.Stan == stan &&
                !x.IsReversed);

        if (trx == null)
            return null;

        trx.IsReversed = true;

        _db.SaveChanges();

        return trx;
    }
    public int SettlementCount()
    {
        return _db.Transactions
            .Count(x =>
                !x.IsReversed &&
                !x.IsVoided &&
                !x.IsSettled);
    }

    public decimal SettlementAmount()
    {
        return _db.Transactions
            .Where(x =>
                !x.IsReversed &&
                !x.IsVoided &&
                !x.IsSettled)
            .Sum(x => x.Amount);
    }
    public Transaction? GetByStan(string stan)
    {
        return _db.Transactions
            .OrderByDescending(x => x.Id)
            .FirstOrDefault(x =>
                x.Stan == stan &&
                !x.IsVoided);
    }
    public void Void(Transaction trx)
    {
        trx.IsVoided = true;
        trx.VoidedAt = DateTime.Now;

        _db.SaveChanges();
    }
    public List<Transaction> GetAll()
    {
        return _db.Transactions
            .OrderByDescending(x => x.Id)
            .ToList();
    }
    public bool ExistsByStan(string stan)
    {
        return _db.Transactions
            .Any(x => x.Stan == stan && !x.IsReversed);
    }
    public void CloseBatch()
    {
    var transactions =
        _db.Transactions
            .Where(x =>
                !x.IsReversed &&
                !x.IsSettled)
            .ToList();

    foreach (var trx in transactions)
    {
        trx.IsSettled = true;
        trx.SettledAt = DateTime.Now;
    }

    _db.SaveChanges();
    }
    public Transaction? GetByRrn(string rrn)
    {
        return _db.Transactions
            .OrderByDescending(x => x.Id)
            .FirstOrDefault(x =>
                x.Rrn == rrn &&
                !x.IsVoided);
    }
}