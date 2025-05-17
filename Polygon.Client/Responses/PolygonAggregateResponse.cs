using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Polygon.Client.Models;

namespace Polygon.Client.Responses
{
    [ExcludeFromCodeCoverage]
    public class PolygonAggregateResponse : PolygonAggregateBaseResponse
    {
        /// <summary>
        /// The exchange symbol that this item is traded under.
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("results")]
        public IEnumerable<Bar> Results { get; set; }
    }
}
