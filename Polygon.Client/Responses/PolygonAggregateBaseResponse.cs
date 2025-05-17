using System.Text.Json.Serialization;

namespace Polygon.Client.Responses
{
    public class PolygonAggregateBaseResponse : PolygonResponseBase
    {
        /// <summary>
        /// The total number of results for this request.
        /// </summary>
        [JsonPropertyName("resultsCount")]
        public int ResultsCount { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// The number of aggregates (minute or day) used to generate the response.
        /// </summary>
        [JsonPropertyName("queryCount")]
        public int QueryCount { get; set; }

        /// <summary>
        /// Whether or not this response was adjusted for splits.
        /// </summary>
        [JsonPropertyName("adjusted")]
        public bool Adjusted { get; set; }

        [JsonPropertyName("next_url")]
        public string NextUrl { get; set; }
    }
}
