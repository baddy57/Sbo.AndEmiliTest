using Sbo.AndEmiliTest.Core.BallDontLieDtos;

namespace Sbo.AndEmiliTest.Core;

internal static class Mappers
{
    internal static Database.NbaPlayerStat ToEntity(this StatsResponse stat)
        => new()
        {
            Id = stat.id,
            Date = stat.game.date,
            GameId = stat.game.id,
            NbaPlayerId = stat.player.id,
            Points = stat.pts ?? 0
        };

    internal static Database.NbaPlayer ToEntity(this Player player)
        => new()
        {
            Id = player.id,
            Name = string.Join(' ', [player.first_name, player.last_name]),
        };
}
