using Microsoft.EntityFrameworkCore;

namespace TechPosHost.Data;

public static class TestDbConnection
{
    public static void Check()
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(
                "Server=DESKTOP-K26B95Q\\SQLEXPRESS;Database=TechPosHost;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;
        using var db = new AppDbContext(options);
        Console.WriteLine($"Database Connected : {db.Database.CanConnect()}");
        //db.Transactions.Add(
        //    new Transaction
        //    {
        //        CreatedAt = DateTime.Now,
        //        MTI = "0200",
        //        ProcessingCode = "000000",
        //        Amount = "1000",
        //        Stan = "999999",
        //        TerminalId = "TERM001",
        //        ResponseCode = "00"
        //    });

        //db.SaveChanges();
    }
}