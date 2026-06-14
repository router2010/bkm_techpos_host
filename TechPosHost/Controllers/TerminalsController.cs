using Microsoft.AspNetCore.Mvc;
using TechPosHost.Data;
using TechPosHost.Dtos;
using TechPosHost.Repository;

namespace TechPosHost.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TerminalsController : ControllerBase
{
    private readonly TerminalRepository _terminalRepository;
    private readonly TransactionRepository _transactionRepository;

    public TerminalsController(TerminalRepository repository, TransactionRepository transactionRepository)
    {
        _terminalRepository = repository;
        _transactionRepository = transactionRepository;
    }
    [HttpGet("{terminalId}/report")]
    public IActionResult Report(
    string terminalId)
    {
        return Ok(
            _transactionRepository
                .TerminalReport(terminalId));
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_terminalRepository.GetAll());
    }

    [HttpPost]
    public IActionResult Create(
        TerminalDto dto)
    {
        if (_terminalRepository.Get(dto.TerminalId) != null)
            return BadRequest("Terminal already exists.");

        _terminalRepository.Add(
            new Terminal
            {
                TerminalId = dto.TerminalId,
                IsActive = dto.IsActive
            });

        return Ok();
    }

    [HttpPut("{terminalId}/enable")]
    public IActionResult Enable(
        string terminalId)
    {
        if (!_terminalRepository.SetActive(terminalId, true))
            return NotFound();

        return Ok();
    }

    [HttpPut("{terminalId}/disable")]
    public IActionResult Disable(
        string terminalId)
    {
        if (!_terminalRepository.SetActive(terminalId, false))
            return NotFound();

        return Ok();
    }
}