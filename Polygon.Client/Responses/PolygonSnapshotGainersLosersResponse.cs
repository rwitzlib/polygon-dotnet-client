using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Polygon.Client.Models;

namespace Polygon.Client.Responses
{
    /// <summary>
    /// API Reference: https://polygon.io/docs/stocks/get_v2_snapshot_locale_us_markets_stocks__direction
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PolygonSnapshotGainersLosersResponse : PolygonResponseBase
    {
        /// <summary>
        /// The total number of results for this request.
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// Array of tickers and their snapshots.
        /// </summary>
        [JsonPropertyName("tickers")]
        public IEnumerable<Snapshot> Tickers { get; set; }
    }
}
