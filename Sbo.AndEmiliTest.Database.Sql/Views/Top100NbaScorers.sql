CREATE VIEW [dbo].[Top100NbaScorers]
	AS SELECT NbaPlayerId, SUM(Points) AS TotalPoints
	FROM NbaPlayerStats
	GROUP BY NbaPlayerId;
