using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Polygon.Client.Requests
{
    /// <summary>
    /// API Reference: https://polygon.io/docs/stocks/get_v2_reference_news
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PolygonTickerNewsRequest
    {
        /// <summary>
        /// Return news articles that match the specified ticker symbol.
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        /// <summary>
        /// Return news articles published on or after this date.
        /// </summary>
        [JsonPropertyName("published_utc")]
        public DateTime? PublishedUtc { get; set; }

        /// <summary>
        /// Sort the results by the specified field. asc or desc. Default is desc.
        /// </summary>
        [JsonPropertyName("sort")]
        public string Sort { get; set; } = "desc";

        /// <summary>
        /// Limit the number of results returned. Default is 100.
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; } = 100;

        /// <summary>
        /// Order the results by the specified field. published_utc or ticker.
        /// </summary>
        [JsonPropertyName("order")]
        public string Order { get; set; } = "published_utc";
    }
}
