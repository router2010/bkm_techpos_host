namespace TechPosHost.Data;

public class Transaction
{
    public int Id { get; set; }

    public string? Pan { get; set; }

    public DateTime CreatedAt { get; set; }

    public string MTI { get; set; } = "";

    public string? ProcessingCode { get; set; }

    public decimal Amount { get; set; }

    public string? Stan { get; set; }

    public string? TerminalId { get; set; }

    public string? ResponseCode { get; set; }

    public bool IsReversed { get; set; }

    public string? Rrn { get; set; }

    public string? AuthCode { get; set; }
    public bool IsSettled { get; set; }

    public DateTime? SettledAt { get; set; }
    public string? TransactionType { get; set; }
    public bool IsVoided { get; set; }

    public DateTime? VoidedAt { get; set; }
}