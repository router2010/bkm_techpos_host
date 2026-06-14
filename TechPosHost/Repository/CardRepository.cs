using TechPosHost.Data;

namespace TechPosHost.Repository;

public class CardRepository
{
    private readonly AppDbContext _db;

    public CardRepository(AppDbContext db)
    {
        _db = db;
    }

    public Card? Get(string pan)
    {
        return _db.Cards
            .FirstOrDefault(x => x.Pan == pan);
    }

    public bool Debit(string pan, decimal amount)
    {
        Console.WriteLine(
            $"DEBIT PAN={pan} AMOUNT={amount}");

        var card = Get(pan);

        if (card == null)
        {
            Console.WriteLine("CARD NOT FOUND");
            return false;
        }

        Console.WriteLine(
            $"BALANCE BEFORE={card.Balance}");

        if (!card.IsActive)
        {
            Console.WriteLine("CARD NOT ACTIVE");
            return false;
        }

        if (card.Balance < amount)
        {
            Console.WriteLine(
                $"INSUFFICIENT FUNDS BALANCE={card.Balance}");
            return false;
        }

        card.Balance -= amount;

        Console.WriteLine(
            $"BALANCE AFTER={card.Balance}");

        _db.SaveChanges();

        Console.WriteLine("SAVECHANGES OK");

        return true;
    }

    public void Credit(string pan, decimal amount)
    {
        var card = Get(pan);

        if (card == null)
            return;

        card.Balance += amount;

        _db.SaveChanges();
    }
}