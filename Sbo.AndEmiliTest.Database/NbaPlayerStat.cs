namespace Sbo.AndEmiliTest.Database;

public partial class NbaPlayerStat
{
    public long Id { get; set; }

    public long NbaPlayerId { get; set; }

    public long GameId { get; set; }

    public DateTime Date { get; set; } = default!;

    public long Points { get; set; }
}
