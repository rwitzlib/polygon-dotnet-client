using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Massive.Client.Models
{
    [ExcludeFromCodeCoverage]
    public class Bar
    {
        /// <summary>
        /// The close price for the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("c")]
        public float Close { get; set; }

        /// <summary>
        /// The highest price for the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("h")]
        public float High { get; set; }

        /// <summary>
        /// The lowest price for the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("l")]
        public float Low { get; set; }

        /// <summary>
        /// The number of transactions in the aggregate window.
        /// </summary>
        [JsonPropertyName("n")]
        public int TransactionCount { get; set; }

        // <summary>
        // The open price for the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("o")]
        public float Open { get; set; }

        // /// <summary>
        // /// Whether or not this aggregate is for an OTC ticker. This field will be left off if false.
        // /// </summary>
        [JsonPropertyName("otc")]
        public bool Otc { get; set; }

        ///// <summary>
        ///// The Unix Msec timestamp for the start of the aggregate window.
        ///// </summary>
        [JsonPropertyName("t")]
        public long Timestamp { get; set; }

        /// <summary>
        /// The trading volume of the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("v")]
        public float Volume { get; set; }

        /// <summary>
        /// The volume weighted average price.
        /// </summary>
        [JsonPropertyName("vw")]
        public float Vwap { get; set; }

        public Bar Clone() => (Bar)MemberwiseClone();
    }
}
