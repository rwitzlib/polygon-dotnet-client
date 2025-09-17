using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Polygon.Client.Models;

namespace Polygon.Client.Responses
{
    /// <summary>
    /// API Reference: https://polygon.io/docs/stocks/get_vx_reference_tickers__id__events
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PolygonTickerEventsResponse : PolygonResponseBase
    {
        /// <summary>
        /// Array of ticker events.
        /// </summary>
        [JsonPropertyName("results")]
        public IEnumerable<TickerEvent> Results { get; set; }

        /// <summary>
        /// The total number of results for this request.
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// If present, this value can be used to fetch the next page of results.
        /// </summary>
        [JsonPropertyName("next_url")]
        public string NextUrl { get; set; }
    }
}
