using System.Text.Json;
using Sbo.AndEmiliTest.Core.Dto;
using Xunit;

namespace Sbo.AndEmiliTest.Core.Tests;

public class DataImporterTests
{
    /// <summary>
    /// this test fails because BallDontLie stats endpoint is not returning the total pages value
    /// </summary>
    [Fact]
    public async Task GetStatsShouldReturnTotalPageNumber_Test()
    {
        using var httpClient = new HttpClient();
        var httpResponse = await httpClient.GetAsync($"https://www.balldontlie.io/api/v1/stats?per_page=10");

        var json = await httpResponse.Content.ReadAsStringAsync();

        var response = JsonSerializer.Deserialize<Root>(json)
            ?? throw new InvalidDataException();

        int totalPages = response.meta.total_pages;

        Assert.NotEqual(0, totalPages);
    }

    [Fact]
    public async Task Top100ShouldContain100Entries_Test()
    {
        var svcs = new BallDontLieServices(new());
        var top100 = await svcs.GetTopScorers(CancellationToken.None);

        Assert.Equal(100, top100.Count());
    }

    [Theory]
    [InlineData(Example)]
    [InlineData(GetTest1)]
    [InlineData(GetTest2)]
    [InlineData(GetTest3)]
    public void GetResponsesShouldBeParsed_Test(string json)
    {
        var deserialized = JsonSerializer.Deserialize<Root>(json);
        Assert.NotNull(deserialized);
    }

    const string Example = """
                        {
              "data": [
                {
                  "id":1,
                  "date":"2018-10-16T00:00:00.000Z",
                  "home_team_score":105,
                  "visitor_team_score":87,
                  "season":2018,
                  "period": 4,
                  "status": "Final",
                  "time": " ",
                  "postseason": false,
                  "home_team":{
                    "id":2,
                    "abbreviation":"BOS",
                    "city":"Boston",
                    "conference":"East",
                    "division":"Atlantic",
                    "full_name":"Boston Celtics",
                    "name":"Celtics"
                  },
                  "visitor_team":{
                    "id":23,
                    "abbreviation":"PHI",
                    "city":"Philadelphia",
                    "conference":"East",
                    "division":"Atlantic",
                    "full_name":"Philadelphia 76ers",
                    "name":"76ers"
                  }
                }
              ],
              "meta": {
                "total_pages": 1877,
                "current_page": 1,
                "next_page": 2,
                "per_page": 25,
                "total_count": 46911
              }
            }
            """;

    const string GetTest1 = """

        {"data":[{"id":737062,"ast":5,"blk":1,"dreb":4,"fg3_pct":0.333,"fg3a":3,"fg3m":1,"fg_pct":0.333,"fga":6,"fgm":2,"ft_pct":1.0,"fta":1,"ftm":1,"game":{"id":32549,"date":"2015-04-05T00:00:00.000Z","home_team_id":27,"home_team_score":107,"period":4,"postseason":false,"season":2014,"status":"Final","time":" ","visitor_team_id":10,"visitor_team_score":92},"min":"22:02","oreb":0,"pf":0,"player":{"id":1412,"first_name":"Manu","height_feet":null,"height_inches":null,"last_name":"Ginobili","position":"","team_id":27,"weight_pounds":null},"pts":6,"reb":4,"stl":0,"team":{"id":27,"abbreviation":"SAS","city":"San Antonio","conference":"West","division":"Southwest","full_name":"San Antonio Spurs","name":"Spurs"},"turnover":2}],
        "meta":{"current_page":1,"next_page":2,"per_page":1}}
        """;

    const string GetTest2 = """
        {"data":[
        {"id":737062,"ast":5,"blk":1,"dreb":4,"fg3_pct":0.333,"fg3a":3,"fg3m":1,"fg_pct":0.333,"fga":6,"fgm":2,"ft_pct":1.0,"fta":1,"ftm":1,"game":{"id":32549,"date":"2015-04-05T00:00:00.000Z","home_team_id":27,"home_team_score":107,"period":4,"postseason":false,"season":2014,"status":"Final","time":" ","visitor_team_id":10,"visitor_team_score":92},"min":"22:02","oreb":0,"pf":0,"player":{"id":1412,"first_name":"Manu","height_feet":null,"height_inches":null,"last_name":"Ginobili","position":"","team_id":27,"weight_pounds":null},"pts":6,"reb":4,"stl":0,"team":{"id":27,"abbreviation":"SAS","city":"San Antonio","conference":"West","division":"Southwest","full_name":"San Antonio Spurs","name":"Spurs"},"turnover":2},
        {"id":737070,"ast":0,"blk":1,"dreb":9,"fg3_pct":0.0,"fg3a":3,"fg3m":0,"fg_pct":0.125,"fga":8,"fgm":1,"ft_pct":1.0,"fta":1,"ftm":1,"game":{"id":32732,"date":"2015-04-05T00:00:00.000Z","home_team_id":12,"home_team_score":112,"period":4,"postseason":false,"season":2014,"status":"Final","time":" ","visitor_team_id":16,"visitor_team_score":89},"min":"29:24","oreb":0,"pf":1,"player":{"id":124,"first_name":"Luol","height_feet":6,"height_inches":9,"last_name":"Deng","position":"F","team_id":18,"weight_pounds":237},"pts":3,"reb":9,"stl":1,"team":{"id":16,"abbreviation":"MIA","city":"Miami","conference":"East","division":"Southeast","full_name":"Miami Heat","name":"Heat"},"turnover":1},
        {"id":737072,"ast":3,"blk":1,"dreb":1,"fg3_pct":0.0,"fg3a":2,"fg3m":0,"fg_pct":0.524,"fga":21,"fgm":11,"ft_pct":0.714,"fta":7,"ftm":5,"game":{"id":32732,"date":"2015-04-05T00:00:00.000Z","home_team_id":12,"home_team_score":112,"period":4,"postseason":false,"season":2014,"status":"Final","time":" ","visitor_team_id":16,"visitor_team_score":89},"min":"33:30","oreb":0,"pf":0,"player":{"id":461,"first_name":"Dwyane","height_feet":6,"height_inches":4,"last_name":"Wade","position":"G","team_id":16,"weight_pounds":220},"pts":27,"reb":1,"stl":2,"team":{"id":16,"abbreviation":"MIA","city":"Miami","conference":"East","division":"Southeast","full_name":"Miami Heat","name":"Heat"},"turnover":4},
        {"id":737073,"ast":5,"blk":0,"dreb":2,"fg3_pct":0.5,"fg3a":2,"fg3m":1,"fg_pct":0.6,"fga":10,"fgm":6,"ft_pct":0.5,"fta":4,"ftm":2,"game":{"id":32732,"date":"2015-04-05T00:00:00.000Z","home_team_id":12,"home_team_score":112,"period":4,"postseason":false,"season":2014,"status":"Final","time":" ","visitor_team_id":16,"visitor_team_score":89},"min":"34:30","oreb":1,"pf":3,"player":{"id":136,"first_name":"Goran","height_feet":6,"height_inches":3,"last_name":"Dragic","position":"G","team_id":17,"weight_pounds":190},"pts":15,"reb":3,"stl":0,"team":{"id":16,"abbreviation":"MIA","city":"Miami","conference":"East","division":"Southeast","full_name":"Miami Heat","name":"Heat"},"turnover":2},
        {"id":737076,"ast":1,"blk":0,"dreb":4,"fg3_pct":0.25,"fg3a":4,"fg3m":1,"fg_pct":0.545,"fga":11,"fgm":6,"ft_pct":1.0,"fta":4,"ftm":4,"game":{"id":32732,"date":"2015-04-05T00:00:00.000Z","home_team_id":12,"home_team_score":112,"period":4,"postseason":false,"season":2014,"status":"Final","time":" ","visitor_team_id":16,"visitor_team_score":89},"min":"27:58","oreb":1,"pf":4,"player":{"id":146,"first_name":"James","height_feet":6,"height_inches":7,"last_name":"Ennis III","position":"F","team_id":8,"weight_pounds":210},"pts":17,"reb":5,"stl":0,"team":{"id":16,"abbreviation":"MIA","city":"Miami","conference":"East","division":"Southeast","full_name":"Miami Heat","name":"Heat"},"turnover":0},
        {"id":737077,"ast":1,"blk":1,"dreb":2,"fg3_pct":0.0,"fg3a":2,"fg3m":0,"fg_pct":0.4,"fga":5,"fgm":2,"ft_pct":0.5,"fta":2,"ftm":1,"game":{"id":32732,"date":"2015-04-05T00:00:00.000Z","home_team_id":12,"home_team_score":112,"period":4,"postseason":false,"season":2014,"status":"Final","time":" ","visitor_team_id":16,"visitor_team_score":89},"min":"13:12","oreb":1,"pf":1,"player":{"id":244,"first_name":"Tyler","height_feet":6,"height_inches":4,"last_name":"Johnson","position":"G","team_id":27,"weight_pounds":190},"pts":5,"reb":3,"stl":1,"team":{"id":16,"abbreviation":"MIA","city":"Miami","conference":"East","division":"Southeast","full_name":"Miami Heat","name":"Heat"},"turnover":0},
        {"id":737081,"ast":1,"blk":0,"dreb":4,"fg3_pct":0.6,"fg3a":5,"fg3m":3,"fg_pct":0.6,"fga":10,"fgm":6,"ft_pct":1.0,"fta":4,"ftm":4,"game":{"id":32732,"date":"2015-04-05T00:00:00.000Z","home_team_id":12,"home_team_score":112,"period":4,"postseason":false,"season":2014,"status":"Final","time":" ","visitor_team_id":16,"visitor_team_score":89},"min":"25:37","oreb":2,"pf":0,"player":{"id":212,"first_name":"Solomon","height_feet":6,"height_inches":7,"last_name":"Hill","position":"F","team_id":20,"weight_pounds":225},"pts":19,"reb":6,"stl":0,"team":{"id":12,"abbreviation":"IND","city":"Indiana","conference":"East","division":"Central","full_name":"Indiana Pacers","name":"Pacers"},"turnover":0},
        {"id":737082,"ast":1,"blk":0,"dreb":5,"fg3_pct":0.0,"fg3a":1,"fg3m":0,"fg_pct":0.2,"fga":5,"fgm":1,"ft_pct":0.833,"fta":12,"ftm":10,"game":{"id":32732,"date":"2015-04-05T00:00:00.000Z","home_team_id":12,"home_team_score":112,"period":4,"postseason":false,"season":2014,"status":"Final","time":" ","visitor_team_id":16,"visitor_team_score":89},"min":"26:22","oreb":1,"pf":2,"player":{"id":1481,"first_name":"David","height_feet":null,"height_inches":null,"last_name":"West","position":"","team_id":19,"weight_pounds":null},"pts":12,"reb":6,"stl":0,"team":{"id":12,"abbreviation":"IND","city":"Indiana","conference":"East","division":"Central","full_name":"Indiana Pacers","name":"Pacers"},"turnover":0}
        ],"meta":{"current_page":1,"next_page":2,"per_page":10}}
        """;

    const string GetTest3 = """
        {"data":[
        {"id":737079,"ast":null,"blk":null,"dreb":null,"fg3_pct":null,"fg3a":null,"fg3m":null,"fg_pct":null,"fga":null,"fgm":null,"ft_pct":null,"fta":null,"ftm":null,"game":{"id":32732,"date":"2015-04-05T00:00:00.000Z","home_team_id":12,"home_team_score":112,"period":4,"postseason":false,"season":2014,"status":"Final","time":" ","visitor_team_id":16,"visitor_team_score":89},"min":null,"oreb":null,"pf":null,"player":{"id":1389,"first_name":"Chris","height_feet":null,"height_inches":null,"last_name":"Andersen","position":"","team_id":8,"weight_pounds":null},"pts":null,"reb":null,"stl":null,"team":{"id":16,"abbreviation":"MIA","city":"Miami","conference":"East","division":"Southeast","full_name":"Miami Heat","name":"Heat"},"turnover":null},
        {"id":737080,"ast":null,"blk":null,"dreb":null,"fg3_pct":null,"fg3a":null,"fg3m":null,"fg_pct":null,"fga":null,"fgm":null,"ft_pct":null,"fta":null,"ftm":null,"game":{"id":32732,"date":"2015-04-05T00:00:00.000Z","home_team_id":12,"home_team_score":112,"period":4,"postseason":false,"season":2014,"status":"Final","time":" ","visitor_team_id":16,"visitor_team_score":89},"min":null,"oreb":null,"pf":null,"player":{"id":474,"first_name":"Hassan","height_feet":7,"height_inches":0,"last_name":"Whiteside","position":"C","team_id":29,"weight_pounds":265},"pts":null,"reb":null,"stl":null,"team":{"id":16,"abbreviation":"MIA","city":"Miami","conference":"East","division":"Southeast","full_name":"Miami Heat","name":"Heat"},"turnover":null}
        ],"meta":{"current_page":1,"next_page":2,"per_page":10}}
        """;
}