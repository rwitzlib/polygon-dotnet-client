using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Massive.Client.Models;

namespace Massive.Client.Responses
{
    [ExcludeFromCodeCoverage]
    public class MassiveAggregateResponse : MassiveAggregateBaseResponse
    {
        /// <summary>
        /// The exchange symbol that this item is traded under.
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("results")]
        public IEnumerable<Bar> Results { get; set; }
    }
}
