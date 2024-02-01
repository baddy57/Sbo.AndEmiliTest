using Microsoft.EntityFrameworkCore;
using Sbo.AndEmiliTest.ApiServer;
using Sbo.AndEmiliTest.Core;
using Sbo.AndEmiliTest.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAndEmiliTestDbContextAndFactory();
builder.Services.AddSingleton<BallDontLieServices>();
builder.Services.AddHostedService<Initializer>();

builder.Services.AddTransient<HttpClient>();
builder.Services.AddLogging(x => x.AddSimpleConsole());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapGet("top100", async (SboAndEmiliTestContext dbContext) =>
{
	var top100 = await dbContext.Top100NbaScorers.ToListAsync();

	if (top100 is null)
		return null;

	return top100;
});

app.MapPut("setfav/{email}/playerid", async (string email,
											int playerid,
											SboAndEmiliTestContext dbContext) =>
{
	var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Email == email) ?? throw new InvalidOperationException();
	var player = await dbContext.NbaPlayers.FindAsync(playerid) ?? throw new InvalidOperationException();

	//todo unique (user-player)
	dbContext.UserFavouritePlayers.Add(new()
	{
		Player = player,
		User = user
	});
});

app.MapPut("unsetfav/{email}/{playerid}", async (string email,
											int playerid,
											SboAndEmiliTestContext dbContext) =>
{
	var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Email == email)
		?? throw new InvalidOperationException();
	var player = await dbContext.NbaPlayers.FindAsync(playerid)
		?? throw new InvalidOperationException();

	//todo unique (user-player)
	var x = await dbContext.UserFavouritePlayers.FirstOrDefaultAsync(x => x.User.Email == email && x.PlayerId == playerid)
		?? throw new InvalidOperationException();

	dbContext.UserFavouritePlayers.Remove(x);
});

app.MapGet("getfavs/{email}", async (string email, SboAndEmiliTestContext dbContext) =>
{
	var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);

	var players = user?.UserFavouritePlayers.Select(x => x.Player)
		?? throw new InvalidOperationException();

	return players;
});

app.MapPut("login/{email}", async (string email, SboAndEmiliTestContext dbContext) =>
{
	if (!await dbContext.Users.AnyAsync(x => x.Email == email))
	{
		dbContext.Users.Add(new() { Email = email });
		await dbContext.SaveChangesAsync();
	}
});

app.Run();
