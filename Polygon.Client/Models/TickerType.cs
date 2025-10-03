using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Polygon.Client.Models
{
    /// <summary>
    /// Represents a ticker type.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TickerType
    {
        /// <summary>
        /// The ticker type code.
        /// </summary>
        [JsonPropertyName("asset_class")]
        public string AssetClass { get; set; }

        /// <summary>
        /// The code for the ticker type.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// The description of the ticker type.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// The locale for this ticker type.
        /// </summary>
        [JsonPropertyName("locale")]
        public string Locale { get; set; }
    }
}
