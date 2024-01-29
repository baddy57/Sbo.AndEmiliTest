using System.Text.Json;
using Sbo.AndEmiliTest.Core.Dto;
using Xunit;

namespace Sbo.AndEmiliTest.Core.Tests;

public class DataImporterTests
{
    [Fact]
    public void ParseJsonTest()
    {
        string json = """
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

        var deserialized = JsonSerializer.Deserialize<Root>(json);
        Assert.NotNull(deserialized);
    }
}