using System.Collections.Generic;
using System.Text.Json.Serialization;
using Polygon.Client.Models;

namespace Polygon.Client.Responses
{
    public class PolygonDailyMarketSummaryResponse : PolygonAggregateBaseResponse
    {
        [JsonPropertyName("results")]
        public List<BarWithTicker> Results { get; set; } = new();
    }
}