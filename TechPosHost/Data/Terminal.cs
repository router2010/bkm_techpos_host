namespace TechPosHost.Data;

public class Terminal
{
    public int Id { get; set; }

    public string TerminalId { get; set; } = "";

    public bool IsActive { get; set; }

    public int? MerchantId { get; set; }

    public Merchant? Merchant { get; set; }
}