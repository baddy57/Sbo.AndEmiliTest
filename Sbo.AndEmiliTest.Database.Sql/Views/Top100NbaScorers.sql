CREATE VIEW [Top100NbaScorers]
	AS SELECT ps.NbaPlayerId, SUM(ps.Points) AS TotalPoints, p.Name
	FROM NbaPlayerStats ps JOIN NbaPlayers p ON ps.NbaPlayerId = p.Id
	GROUP BY ps.NbaPlayerId
	ORDER BY TotalPoints DESC
	LIMIT 100