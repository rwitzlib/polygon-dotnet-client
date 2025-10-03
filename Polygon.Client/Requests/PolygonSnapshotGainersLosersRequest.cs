using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Polygon.Client.Requests
{
    /// <summary>
    /// API Reference: https://polygon.io/docs/stocks/get_v2_snapshot_locale_us_markets_stocks__direction
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PolygonSnapshotGainersLosersRequest
    {
        /// <summary>
        /// The direction of the snapshot. Either "gainers" or "losers".
        /// </summary>
        [JsonPropertyName("direction")]
        public string Direction { get; set; }

        /// <summary>
        /// Include OTC securities in the response. Defaults to false.
        /// </summary>
        [JsonPropertyName("include_otc")]
        public bool IncludeOtc { get; set; } = false;

        /// <summary>
        /// Limit the number of results returned. Defaults to 20, maximum is 1000.
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; } = 20;
    }
}
