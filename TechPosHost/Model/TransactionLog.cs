namespace TechPosHost.Models;

public class TransactionLog
{
    public DateTime CreatedAt { get; set; }
    public string? Pan { get; set; }
    public string MTI { get; set; } = "";

    public string? ProcessingCode { get; set; }

    public decimal Amount { get; set; }

    public string? Stan { get; set; }

    public string? TerminalId { get; set; }

    public string? ResponseCode { get; set; }
    public string? Rrn { get; set; }

    public string? AuthCode { get; set; }
    public string? TransactionType { get; set; }
}