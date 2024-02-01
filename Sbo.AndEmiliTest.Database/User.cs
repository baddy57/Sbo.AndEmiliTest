using System;
using System.Collections.Generic;

namespace Sbo.AndEmiliTest.Database;

public partial class User
{
    public long Id { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<UserFavouritePlayer> UserFavouritePlayers { get; set; } = new List<UserFavouritePlayer>();
}
