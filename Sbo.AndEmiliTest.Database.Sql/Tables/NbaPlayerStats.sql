CREATE TABLE [dbo].[NbaPlayerStats]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[NbaPlayerId] INT NOT NULL,
	[GameId] INT NOT NULL,
	[Date] DATE NOT NULL,
	[Points] INT NOT NULL
)
