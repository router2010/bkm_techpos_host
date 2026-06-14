using Microsoft.AspNetCore.Mvc;
using TechPosHost.Dto;
using TechPosHost.Repository;

namespace TechPosHost.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MerchantController : ControllerBase
{
    private readonly MerchantRepository _merchantRepository;

    public MerchantController(
        MerchantRepository merchantRepository)
    {
        _merchantRepository = merchantRepository;
    }

    [HttpGet("{merchantNo}/settlement")]
    public IActionResult Settlement(
        string merchantNo)
    {
        var merchant =
            _merchantRepository.Get(merchantNo);

        if (merchant == null)
            return NotFound();

        return Ok(
            new MerchantSettlementDto
            {
                MerchantNo = merchant.MerchantNo,
                MerchantName = merchant.Name,
                Count = _merchantRepository
                    .SettlementCount(merchantNo),
                Amount = _merchantRepository
                    .SettlementAmount(merchantNo)
            });
    }
}