namespace TechPosHost.Data;

public class Terminal
{
    public int Id { get; set; }

    public string TerminalId { get; set; } = "";

    public string? MerchantId { get; set; }

    public bool IsActive { get; set; }
}
