using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json.Serialization;
using Polygon.Client.Models;

namespace Polygon.Client.Responses
{
    [ExcludeFromCodeCoverage]
    public class PolygonTickerDetailsResponse : PolygonResponseBase
    {
        /// <summary>
        /// The total number of results for this request.
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// Ticker with details.
        /// </summary>
        [JsonPropertyName("results")]
        public TickerDetails TickerDetails { get; set; }
    }
}
