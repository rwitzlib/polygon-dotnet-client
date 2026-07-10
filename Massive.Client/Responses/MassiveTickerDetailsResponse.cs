using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Massive.Client.Models;

namespace Massive.Client.Responses
{
    [ExcludeFromCodeCoverage]
    public class MassiveTickerDetailsResponse : MassiveResponseBase
    {
        /// <summary>
        /// The total number of results for this request.
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// Ticker with details.
        /// </summary>
        [JsonPropertyName("results")]
        public TickerDetails TickerDetails { get; set; }
    }
}
