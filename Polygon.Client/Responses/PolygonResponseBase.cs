using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Polygon.Client.Responses
{
    public abstract class PolygonResponseBase
    {
        /// <summary>
        /// A request id assigned by the server.
        /// </summary>
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }

        /// <summary>
        /// The status of this request's response.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
} 