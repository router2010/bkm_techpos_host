namespace TechPosHost.Dto;

public class TerminalReportDto
{
    public string TerminalId { get; set; } = "";

    public int SaleCount { get; set; }

    public decimal SaleAmount { get; set; }

    public int RefundCount { get; set; }

    public decimal RefundAmount { get; set; }

    public int VoidCount { get; set; }

    public decimal VoidAmount { get; set; }
}
