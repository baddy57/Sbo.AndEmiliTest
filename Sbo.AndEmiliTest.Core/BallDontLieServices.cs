using System.Text.Json;
using Sbo.AndEmiliTest.Core.Dto;

namespace Sbo.AndEmiliTest.Core;

public class BallDontLieServices
{
    private readonly HttpClient httpClient;

    public BallDontLieServices(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <summary>
    /// Retrieves the 100 top nba scorers for the current season
    /// </summary>
    public async Task<IEnumerable<(Player Player, int Points)>> GetTopScorers(CancellationToken ct)
    {
        List<(Player Player, int Points)> allStats = [];

        for (int page = 0, totalPages = int.MaxValue;
            page < totalPages && !ct.IsCancellationRequested;
            page++)
        {
            var httpResponse = await httpClient.GetAsync($"https://www.balldontlie.io/api/v1/stats?page={page}&per_page=100", ct);

            var json = await httpResponse.Content.ReadAsStringAsync(ct);

            var response = JsonSerializer.Deserialize<Root>(json)
                ?? throw new InvalidDataException();

            totalPages = response.meta.total_pages;

            var stats = response.data
                .Where(x => x.pts is not null)
                .Select(datum => (datum.player, datum.pts!.Value));

            allStats.AddRange(stats);
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
    private static IEnumerable<(Player player, int totalPoints)> AggregatePoints(List<(Player Player, int Points)> data)
        => data.GroupBy(x => x.Player.id)
            .Select(x =>
            {
                var player = x.First().Player;
                var totalPoints = x.Sum(tuple => tuple.Points);
                return (player, totalPoints);
            });
}
