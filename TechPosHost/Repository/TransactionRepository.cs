using Microsoft.EntityFrameworkCore;
using TechPosHost.Data;
using TechPosHost.Models;

namespace TechPosHost.Repository;

public class TransactionRepository
{
    private readonly string _connectionString =
        "Server=DESKTOP-K26B95Q\\SQLEXPRESS;Database=TechPosHost;Trusted_Connection=True;TrustServerCertificate=True;";

    public void Add(TransactionLog log)
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using var db = new AppDbContext(options);

        db.Transactions.Add(
            new Transaction
            {
                CreatedAt = log.CreatedAt,
                MTI = log.MTI,
                ProcessingCode = log.ProcessingCode,
                Amount = log.Amount,
                Stan = log.Stan,
                TerminalId = log.TerminalId,
                ResponseCode = log.ResponseCode
            });

        db.SaveChanges();
    }

    public int Count()
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using var db = new AppDbContext(options);

        return db.Transactions.Count();
    }
    public bool Reverse(string stan)
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        using var db = new AppDbContext(options);

        var trx = db.Transactions
            .OrderByDescending(x => x.Id)
            .FirstOrDefault(x =>
                x.Stan == stan &&
                !x.IsReversed);

        if (trx == null)
        {
            return false;
        }

        trx.IsReversed = true;

        db.SaveChanges();

        return true;
    }
}