using TechPosHost.Data;
namespace TechPosHost.Data;

public class Merchant
{
    public int Id { get; set; }

    public string MerchantNo { get; set; } = "";

    public string Name { get; set; } = "";

    public bool IsActive { get; set; }
}
