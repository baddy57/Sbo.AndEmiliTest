using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sbo.AndEmiliTest.Database;

namespace Sbo.AndEmiliTest.ApiServer.Controllers;

[ApiController]
[Route("[players]")]
public class PlayersController : ControllerBase
{
    private readonly SboAndEmiliTestContext dbContext;
    private readonly ILogger<PlayersController> _logger;

    public PlayersController(ILogger<PlayersController> logger, SboAndEmiliTestContext dbContext)
    {
        _logger = logger;
        this.dbContext = dbContext;
    }

    [HttpGet(Name = "GetTop100")]
    public async Task<ActionResult<IEnumerable<Top100NbaScorer>>> GetTop100(string username)
    {
        var top100 = await dbContext.Top100NbaScorers.ToListAsync();

        foreach (var item in top100)
        {
            await dbContext.NbaPlayers.FindAsync(item.NbaPlayerId);
        }

        var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Email == username);
        var players = user?.UserFavouritePlayers.Select(x => x.Player);

        if (players is null)
            return NotFound();

        return Ok(players);
    }
}
