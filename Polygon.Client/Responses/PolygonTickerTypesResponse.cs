using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Polygon.Client.Models;

namespace Polygon.Client.Responses
{
    /// <summary>
    /// API Reference: https://polygon.io/docs/stocks/get_v3_reference_tickers_types
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PolygonTickerTypesResponse : PolygonResponseBase
    {
        /// <summary>
        /// The total number of results for this request.
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// Array of ticker types.
        /// </summary>
        [JsonPropertyName("results")]
        public IEnumerable<TickerType> Results { get; set; }
    }
}
