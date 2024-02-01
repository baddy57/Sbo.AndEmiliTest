using Sbo.AndEmiliTest.Core.BallDontLieDtos;

namespace Sbo.AndEmiliTest.Core;

internal static class Mappers
{
    internal static Database.NbaPlayerStat ToEntity(this StatsResponse stat)
        => new Database.NbaPlayerStat()
        {
            Id = stat.id,
            Date = stat.game.date,
            GameId = stat.game.id,
            NbaPlayerId = stat.player.id,
            Points = stat.pts ?? 0
        };
}
