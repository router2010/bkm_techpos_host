namespace TechPosHost.Dto;

public class MerchantSettlementDto
{
    public string MerchantNo { get; set; } = "";

    public string MerchantName { get; set; } = "";

    public int Count { get; set; }

    public decimal Amount { get; set; }
}