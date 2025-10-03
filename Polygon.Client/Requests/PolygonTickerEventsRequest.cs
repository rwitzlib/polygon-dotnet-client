using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Polygon.Client.Requests
{
    /// <summary>
    /// API Reference: https://polygon.io/docs/stocks/get_vx_reference_tickers__id__events
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PolygonTickerEventsRequest
    {
        /// <summary>
        /// The ticker identifier to get events for.
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        /// <summary>
        /// The types of events to return. If not specified, all event types are returned.
        /// Possible values: status_change, name_change, ticker_change, etc.
        /// </summary>
        [JsonPropertyName("types")]
        public string Types { get; set; }

        /// <summary>
        /// Filter for events on or after this date.
        /// </summary>
        [JsonPropertyName("date_from")]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Filter for events on or before this date.
        /// </summary>
        [JsonPropertyName("date_to")]
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Sort the results by timestamp. asc or desc. Default is desc.
        /// </summary>
        [JsonPropertyName("sort")]
        public string Sort { get; set; } = "desc";

        /// <summary>
        /// Limit the number of results returned. Default is 100.
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; } = 100;
    }
}
