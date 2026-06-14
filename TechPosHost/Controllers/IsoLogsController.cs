using Microsoft.AspNetCore.Mvc;
using TechPosHost.Repository;

namespace TechPosHost.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IsoLogsController : ControllerBase
{
    private readonly IsoLogRepository _repository;

    public IsoLogsController(
        IsoLogRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_repository.GetAll());
    }
}
