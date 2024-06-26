﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Polygon.Client.Models;

namespace Polygon.Client.Responses
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PolygonGetTickersResponse
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("results")]
        public IEnumerable<TickerDetails> Results { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("next_url")]
        public string NextUrl { get; set; }
    }
}
