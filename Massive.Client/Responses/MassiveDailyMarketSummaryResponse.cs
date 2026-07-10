using System.Collections.Generic;
using System.Text.Json.Serialization;
using Massive.Client.Models;

namespace Massive.Client.Responses
{
    public class MassiveDailyMarketSummaryResponse : MassiveAggregateBaseResponse
    {
        [JsonPropertyName("results")]
        public List<BarWithTicker> Results { get; set; } = new();
    }
}
