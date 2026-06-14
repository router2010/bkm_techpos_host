namespace TechPosHost.Data;

public class IsoLog
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Request { get; set; } = "";

    public string Response { get; set; } = "";

    public string? MTI { get; set; }

    public string? TerminalId { get; set; }
}