using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sbo.AndEmiliTest.Database;

namespace Sbo.AndEmiliTest.ApiServer.Controllers;

[ApiController]
[Route("players")]
public class PlayersController : ControllerBase
{
    private readonly SboAndEmiliTestContext dbContext;
    private readonly ILogger<PlayersController> _logger;

    public PlayersController(ILogger<PlayersController> logger, SboAndEmiliTestContext dbContext)
    {
        _logger = logger;
        this.dbContext = dbContext;
    }

    [HttpGet("top100", Name = "GetTop100")]
    public async Task<ActionResult<IEnumerable<Top100NbaScorer>>> GetTop100()
    {
        var top100 = await dbContext.Top100NbaScorers.ToListAsync();

        if (top100 is null)
            return NotFound();

        return Ok(top100);
    }
}
