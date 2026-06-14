using Microsoft.AspNetCore.Mvc;
using TechPosHost.Data;
using TechPosHost.Dtos;
using TechPosHost.Repository;

namespace TechPosHost.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TerminalsController : ControllerBase
{
    private readonly TerminalRepository _repository;

    public TerminalsController(
        TerminalRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_repository.GetAll());
    }

    [HttpPost]
    public IActionResult Create(
        TerminalDto dto)
    {
        if (_repository.Get(dto.TerminalId) != null)
            return BadRequest("Terminal already exists.");

        _repository.Add(
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
        if (!_repository.SetActive(terminalId, true))
            return NotFound();

        return Ok();
    }

    [HttpPut("{terminalId}/disable")]
    public IActionResult Disable(
        string terminalId)
    {
        if (!_repository.SetActive(terminalId, false))
            return NotFound();

        return Ok();
    }
}