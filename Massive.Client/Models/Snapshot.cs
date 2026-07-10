using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Massive.Client.Models
{
    /// <summary>
    /// API Reference: https://massive.com/docs/rest/stocks/snapshots/all-tickers
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Snapshot
    {
        /// <summary>
        /// The most recent daily bar for this ticker.
        /// </summary>
        [JsonPropertyName("day")]
        public Bar Day { get; set; }

        /// <summary>
        /// The most recent minute bar for this ticker.
        /// </summary>
        [JsonPropertyName("min")]
        public Bar Minute { get; set; }

        /// <summary>
        /// The previous day's bar for this ticker.
        /// </summary>
        [JsonPropertyName("prevDay")]
        public Bar PreviousDay { get; set; }

        /// <summary>
        /// The exchange symbol that this item is traded under.
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        /// <summary>
        /// The value of the change from the previous day.
        /// </summary>
        [JsonPropertyName("todaysChange")]
        public float TodaysChange { get; set; }

        /// <summary>
        /// The percentage change since the previous day.
        /// </summary>
        [JsonPropertyName("todaysChangePerc")]
        public float TodaysChangePercentage { get; set; }

        /// <summary>
        /// The last updated timestamp.
        /// </summary>
        [JsonPropertyName("updated")]
        public long Updated { get; set; }
    }
}
