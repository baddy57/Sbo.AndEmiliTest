using System;
using System.Collections.Generic;

namespace Sbo.AndEmiliTest.Database;

public partial class NbaPlayerStat
{
    public long Id { get; set; }

    public long NbaPlayerId { get; set; }

    public long GameId { get; set; }

    public byte[] Date { get; set; } = null!;

    public long Points { get; set; }
}
