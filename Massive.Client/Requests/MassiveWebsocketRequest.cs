using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Massive.Client.Requests
{
    [ExcludeFromCodeCoverage]
    public class MassiveWebsocketRequest
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("params")]
        public string Params { get; set; }
    }
}
