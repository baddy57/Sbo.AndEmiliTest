using System;
using System.Collections.Generic;

namespace Sbo.AndEmiliTest.Database;

public partial class UserFavouritePlayer
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long PlayerId { get; set; }

    public virtual NbaPlayer Player { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
