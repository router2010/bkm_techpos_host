using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using TechPosHost.Repository;
using TechPosHost.Dtos;
namespace TechPosHost.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionRepository _repository;

    public TransactionsController(
        TransactionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_repository.GetAll());
    }

    [HttpGet("{stan}")]
    public IActionResult GetByStan(string stan)
    {
        var trx = _repository.GetByStan(stan);

        if (trx == null)
            return NotFound();

        return Ok(
            new TransactionInquiryDto
            {
                Stan = trx.Stan,
                Amount = trx.Amount,
                Rrn = trx.Rrn,
                AuthCode = trx.AuthCode,
                ResponseCode = trx.ResponseCode,
                IsReversed = trx.IsReversed
            });
    }

    [HttpGet("/api/settlement")]
    public IActionResult Settlement()
    {
        return Ok(new
        {
            Count = _repository.SettlementCount(),
            Amount = _repository.SettlementAmount()
        });
    }
}