﻿using Polygon.Client.Contracts.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json.Serialization;

namespace Polygon.Clients.Contracts.Responses
{
    [ExcludeFromCodeCoverage]
    public class PolygonAggregateResponse
    {
        /// <summary>
        /// The exchange symbol that this item is traded under.
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        /// <summary>
        /// Whether or not this response was adjusted for splits.
        /// </summary>
        [JsonPropertyName("adjusted")]
        public bool Adjusted { get; set; }

        /// <summary>
        /// The number of aggregates (minute or day) used to generate the response.
        /// </summary>
        [JsonPropertyName("queryCount")]
        public int QueryCount { get; set; }

        /// <summary>
        /// A request id assigned by the server.
        /// </summary>
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }

        /// <summary>
        /// The total number of results for this request.
        /// </summary>
        [JsonPropertyName("resultsCount")]
        public int ResultsCount { get; set; }

        /// <summary>
        /// The status of this request's response.
        /// </summary>
        [JsonPropertyName("status")]
        public HttpStatusCode Status { get; set; }

        [JsonPropertyName("results")]
        public IEnumerable<Bar> Results { get; set; }

        [JsonPropertyName("next_url")]
        public string NextUrl { get; set; }
    }
}
