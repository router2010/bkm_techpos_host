namespace TechPosHost.Data;

public class Card
{
    public int Id { get; set; }

    public string Pan { get; set; } = "";

    public decimal Balance { get; set; }

    public bool IsActive { get; set; }
}