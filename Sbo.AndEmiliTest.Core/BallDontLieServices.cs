using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sbo.AndEmiliTest.Core.BallDontLieDtos;
using Sbo.AndEmiliTest.Database;

namespace Sbo.AndEmiliTest.Core;

public class BallDontLieServices
{
    private readonly HttpClient httpClient;
    private ILogger<BallDontLieServices> logger;
    private IDbContextFactory<SboAndEmiliTestContext> dbContextFactory;

    public BallDontLieServices(HttpClient httpClient, ILogger<BallDontLieServices> logger, IDbContextFactory<SboAndEmiliTestContext> dbContextFactory)
    {
        this.httpClient = httpClient;
        this.logger = logger;
        this.dbContextFactory = dbContextFactory;
    }

    /// <summary>
    /// pulls current season stats and players from balldontlie and stores them in the local db
    /// </summary>
    public async Task InitializePlayersAndStats(CancellationToken ct)
    {
        //exclude latest data to avoid handling games being played
        var maxDate = (DateTime.Now - TimeSpan.FromDays(2)).Date;

        //avoid re-dowloading data
        var minDate = DateTime.MinValue.Date;
        using (var context = dbContextFactory.CreateDbContext())
        {
            minDate = (await context.NbaPlayerStats.OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync(ct))?.Date - TimeSpan.FromDays(1) ?? DateTime.MinValue.Date;
        }

        logger.LogInformation("Beginning data download...");

        int page = 0;
        while (!ct.IsCancellationRequested)
        {
            var httpResponse = await httpClient.GetAsync($"https://www.balldontlie.io/api/v1/stats?page={page}&per_page={100}&seasons[]=2023&postseason=false&end_date={maxDate.ToString("yyyy-MM-dd")}&start_date={minDate.ToString("yyyy-MM-dd")}", ct);
            var json = await httpResponse.Content.ReadAsStringAsync(ct);

            //todo better retry policy
            //todo store fetched data and download only newer stats
            //todo sql view w/ aggregate points
            if (json.Equals("Retry later\n"))
            {
                await Task.Delay(5000, ct);
                logger.LogWarning("Api call limit reached {page}", page);
                continue;
            }

            var response = JsonSerializer.Deserialize<Root>(json)
                ?? throw new InvalidDataException("Could not parse data");
            if (response.data.Count == 0)
                break;

            logger.LogInformation("Downloaded {count} records", response.data.Count);

            var stats = response.data
                .Where(x => x.pts is not null)
                .Select(x => x.ToEntity());
            var players = response.data.Select(x => x.player.ToEntity())
                        .DistinctBy(x => x.Id);

            using (var context = await dbContextFactory.CreateDbContextAsync(ct))
            {
                foreach (var player in players)
                {
                    if (!context.NbaPlayers.Contains(player))
                        context.NbaPlayers.Add(player);
                }

                foreach (var stat in stats)
                {
                    if (!context.NbaPlayerStats.Contains(stat))
                        context.NbaPlayerStats.Add(stat);
                }
                //context.NbaPlayers.AddRangeIfNotExists(players, x => x.Id);
                //context.NbaPlayerStats.AddRangeIfNotExists(stats, x => x.Id);
                await context.SaveChangesAsync(ct);
            }

            if (response.meta.next_page is null)
                break;

            page++;
        }
        logger.LogInformation("Player stats downloaded");
    }

    /// <summary>
    /// Retrieves the 100 top nba scorers for the current season
    /// </summary>
    [Obsolete]
    public async Task<IEnumerable<(Player Player, int Points)>> GetTopScorers(CancellationToken ct)
    {
        List<(Player Player, int Points)> allStats = [];

        int page = 0;
        while (!ct.IsCancellationRequested)
        {
            logger.LogTrace("Fetching page {page}", page);

            var httpResponse = await httpClient.GetAsync($"https://www.balldontlie.io/api/v1/stats?page={page}&per_page={100}&seasons[]=2023&postseason=false", ct);

            var json = await httpResponse.Content.ReadAsStringAsync(ct);

            //todo better retry policy
            //todo store fetched data and download only newer stats
            //todo sql view w/ aggregate points
            if (json.Equals("Retry later\n"))
            {
                await Task.Delay(5000);
                logger.LogWarning("Api call limit reached {page}", page);
                continue;
            }

            var response = JsonSerializer.Deserialize<Root>(json)
                ?? throw new InvalidDataException("Could not parse data");
            if (response.data.Count == 0)
                break;

            var stats = response.data
                .Where(x => x.pts is not null)
                .Select(datum => (datum.player, datum.pts!.Value));

            allStats.AddRange(stats);

            if (response.meta.next_page is null)
                break;

            page++;
            //await Task.Delay(1000);
        }

        var top100 = AggregatePoints(allStats)
            .OrderBy(x => x.totalPoints)
            .Take(100);

        return top100;
    }

    /// <summary>
    /// aggregates points by player id
    /// </summary>
    /// <returns>a list of unique players and their total points</returns>
    [Obsolete]
    private static IEnumerable<(Player player, int totalPoints)> AggregatePoints(List<(Player Player, int Points)> data)
        => data.GroupBy(x => x.Player.id)
            .Select(x =>
            {
                var player = x.First().Player;
                var totalPoints = x.Sum(tuple => tuple.Points);
                return (player, totalPoints);
            });
}
