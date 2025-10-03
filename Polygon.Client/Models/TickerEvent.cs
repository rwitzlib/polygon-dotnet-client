using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Polygon.Client.Models
{
    /// <summary>
    /// Represents a ticker event.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TickerEvent
    {
        /// <summary>
        /// The type of event.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// The date of the event.
        /// </summary>
        [JsonPropertyName("date")]
        public long Date { get; set; }

        /// <summary>
        /// Additional details about the event.
        /// </summary>
        [JsonPropertyName("details")]
        public object Details { get; set; }

        /// <summary>
        /// The ticker symbol associated with the event.
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        /// <summary>
        /// The ticker symbol before the change (if applicable).
        /// </summary>
        [JsonPropertyName("ticker_from")]
        public string TickerFrom { get; set; }

        /// <summary>
        /// The ticker symbol after the change (if applicable).
        /// </summary>
        [JsonPropertyName("ticker_to")]
        public string TickerTo { get; set; }

        /// <summary>
        /// The name before the change (if applicable).
        /// </summary>
        [JsonPropertyName("name_from")]
        public string NameFrom { get; set; }

        /// <summary>
        /// The name after the change (if applicable).
        /// </summary>
        [JsonPropertyName("name_to")]
        public string NameTo { get; set; }
    }
}
