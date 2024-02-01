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

	[HttpGet("setfav")]
	public async Task<ActionResult> SetFavourite(string email, int playerid)
	{
		var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);
		if (user is null)
			return BadRequest();

		var player = await dbContext.NbaPlayers.FindAsync(playerid);
		if (player is null)
			return BadRequest();

		//todo unique (user-player)
		dbContext.UserFavouritePlayers.Add(new()
		{
			Player = player,
			User = user
		});

		return Ok();
	}

	[HttpGet("unsetfav")]
	public async Task<ActionResult> UnSetFavourite(string email, int playerid)
	{
		var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);
		if (user is null)
			return BadRequest();

		var player = await dbContext.NbaPlayers.FindAsync(playerid);
		if (player is null)
			return BadRequest();

		var x = await dbContext.UserFavouritePlayers.FirstOrDefaultAsync(x => x.User.Email == email && x.PlayerId == playerid);
		if (x is null)
			return BadRequest();

		dbContext.UserFavouritePlayers.Remove(x);

		return Ok();
	}
}
