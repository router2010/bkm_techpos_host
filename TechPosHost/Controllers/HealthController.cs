using Microsoft.AspNetCore.Mvc;
using TechPosHost.Data;

namespace TechPosHost.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _db;

    public HealthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            bool dbUp = _db.Database.CanConnect();

            return Ok(new
            {
                Status = "UP",
                Database = dbUp ? "UP" : "DOWN",
                ServerTime = DateTime.Now
            });
        }
        catch
        {
            return StatusCode(500, new
            {
                Status = "DOWN",
                Database = "DOWN"
            });
        }
    }
}