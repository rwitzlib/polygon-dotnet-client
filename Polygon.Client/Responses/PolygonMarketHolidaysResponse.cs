using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Polygon.Client.Models;

namespace Polygon.Client.Responses
{
    /// <summary>
    /// API Reference: https://polygon.io/docs/stocks/get_v1_marketstatus_upcoming
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PolygonMarketHolidaysResponse : PolygonResponseBase
    {
        /// <summary>
        /// Array of market holidays.
        /// </summary>
        [JsonPropertyName("")]
        public IEnumerable<MarketHoliday> Results { get; set; }
    }
}
