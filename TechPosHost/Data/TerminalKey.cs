namespace TechPosHost.Data;

public class TerminalKey
{
    public int Id { get; set; }

    public string TerminalId { get; set; } = "";

    public string TMK { get; set; } = "";

    public string TPK { get; set; } = "";

    public string TAK { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}
