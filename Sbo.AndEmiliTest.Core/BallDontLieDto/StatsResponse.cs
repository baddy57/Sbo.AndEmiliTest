namespace Sbo.AndEmiliTest.Core.Dto;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class StatsResponse
{
    public int id { get; set; }
    public int? ast { get; set; }
    public int? blk { get; set; }
    public int? dreb { get; set; }
    public double? fg3_pct { get; set; }
    public int? fg3a { get; set; }
    public int? fg3m { get; set; }
    public double? fg_pct { get; set; }
    public int? fga { get; set; }
    public int? fgm { get; set; }
    public double? ft_pct { get; set; }
    public int? fta { get; set; }
    public int? ftm { get; set; }
    public Game game { get; set; }
    public string? min { get; set; }
    public int? oreb { get; set; }
    public int? pf { get; set; }
    public Player player { get; set; }
    public int? pts { get; set; }
    public int? reb { get; set; }
    public int? stl { get; set; }
    public Team team { get; set; }
    public int? turnover { get; set; }
}
