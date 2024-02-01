CREATE TABLE [dbo].[UserFavouritePlayers]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] INT NOT NULL, 
    [PlayerId] INT NOT NULL, 
    CONSTRAINT [FK_UserFavouritePlayers_ToUsers] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_UserFavouritePlayers_ToNbaPlayers] FOREIGN KEY ([PlayerId]) REFERENCES [NbaPlayers]([Id])
)
