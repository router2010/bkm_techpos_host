using Microsoft.AspNetCore.Mvc;
using TechPosHost.Repository;

namespace TechPosHost.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly TransactionRepository _transactionRepository;

    public ReportController(
        TransactionRepository transactionRepository)
    {
        _transactionRepository =
            transactionRepository;
    }

    [HttpGet("daily")]
    public IActionResult Daily()
    {
        return Ok(
            _transactionRepository.DailyReport());
    }
}