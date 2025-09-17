using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Polygon.Client.Models
{
    /// <summary>
    /// Represents a market holiday.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MarketHoliday
    {
        /// <summary>
        /// The date of the holiday.
        /// </summary>
        [JsonPropertyName("date")]
        public long Date { get; set; }

        /// <summary>
        /// The exchange that the holiday applies to.
        /// </summary>
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        /// <summary>
        /// The name of the holiday.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The status of the market on this date.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// The settlement date if applicable.
        /// </summary>
        [JsonPropertyName("settlement_date")]
        public long? SettlementDate { get; set; }
    }
}
