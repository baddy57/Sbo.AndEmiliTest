using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sbo.AndEmiliTest.Database;

namespace Sbo.AndEmiliTest.ApiServer.Controllers;

[ApiController]
[Route("[users]")]
public class UsersController : ControllerBase
{
    private readonly SboAndEmiliTestContext dbContext;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger, SboAndEmiliTestContext dbContext)
    {
        _logger = logger;
        this.dbContext = dbContext;
    }

    [HttpGet(Name = "GetUserFavs")]
    public async Task<ActionResult<IEnumerable<NbaPlayer>>> GetUserFavourites(string email)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);
        var players = user?.UserFavouritePlayers.Select(x => x.Player);

        if (players is null)
            return NotFound();

        return Ok(players);
    }

    [HttpGet(Name = "Login")]
    public async Task<ActionResult> Login(string email)
    {
        if (await dbContext.Users.AnyAsync(x => x.Email == email))
            return Ok();
        dbContext.Users.Add(new() { Email = email });
        await dbContext.SaveChangesAsync();
        return Ok();
    }
}
