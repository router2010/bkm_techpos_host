namespace TechPosHost.Dtos;

public class TransactionInquiryDto
{
    public string? Stan { get; set; }

    public string? Amount { get; set; }

    public string? Rrn { get; set; }

    public string? AuthCode { get; set; }

    public string? ResponseCode { get; set; }

    public bool IsReversed { get; set; }
}
