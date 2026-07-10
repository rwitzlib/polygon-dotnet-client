using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Massive.Client.Models;

namespace Massive.Client.Responses
{
    [ExcludeFromCodeCoverage]
    public class MassiveGetTickersResponse
    {
        [JsonPropertyName("results")]
        public IEnumerable<TickerDetails> Results { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("next_url")]
        public string NextUrl { get; set; }
    }
}
