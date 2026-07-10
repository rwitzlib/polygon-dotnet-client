using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Massive.Client.Responses
{
    [ExcludeFromCodeCoverage]
    public class MassiveWebsocketAggregateResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// The event type.
        /// </summary>
        [JsonPropertyName("ev")]
        public string Event { get; set; }

        /// <summary>
        /// The ticker symbol for the given stock.
        /// </summary>
        [JsonPropertyName("sym")]
        public string Ticker { get; set; }

        /// <summary>
        /// The tick volume.
        /// </summary>
        [JsonPropertyName("v")]
        public float Volume { get; set; }

        /// <summary>
        /// Today's accumulated volume.
        /// </summary>
        [JsonPropertyName("av")]
        public float AccumulatedVolume { get; set; }

        /// <summary>
        /// Today's official opening price.
        /// </summary>
        [JsonPropertyName("op")]
        public float OpeningPrice { get; set; }

        /// <summary>
        /// The tick's volume weighted average price.
        /// </summary>
        [JsonPropertyName("vw")]
        public float TickVwap { get; set; }

        /// <summary>
        /// The opening tick price for this aggregate window.
        /// </summary>
        [JsonPropertyName("o")]
        public float Open { get; set; }

        /// <summary>
        /// The closing tick price for this aggregate window.
        /// </summary>
        [JsonPropertyName("c")]
        public float Close { get; set; }

        /// <summary>
        /// The highest tick price for this aggregate window.
        /// </summary>
        [JsonPropertyName("h")]
        public float High { get; set; }

        /// <summary>
        /// The lowest tick price for this aggregate window.
        /// </summary>
        [JsonPropertyName("l")]
        public float Low { get; set; }

        /// <summary>
        /// Today's volume weighted average price.
        /// </summary>
        [JsonPropertyName("a")]
        public float DayVwap { get; set; }

        /// <summary>
        /// The average trade size for this aggregate window.
        /// </summary>
        [JsonPropertyName("z")]
        public float AverageTradeSize { get; set; }

        /// <summary>
        /// The timestamp of the starting tick for this aggregate window in Unix Milliseconds.
        /// </summary>
        [JsonPropertyName("s")]
        public long TickStart { get; set; }

        /// <summary>
        /// The timestamp of the ending tick for this aggregate window in Unix Milliseconds.
        /// </summary>
        [JsonPropertyName("e")]
        public long TickEnd { get; set; }
    }
}
