using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Massive.Client.Models;

namespace Massive.Client.Responses
{
    /// <summary>
    /// API Reference: https://massive.com/docs/rest/stocks/snapshots/all-tickers
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MassiveSnapshotResponse : MassiveResponseBase
    {
        /// <summary>
        /// The total number of results for this request.
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("tickers")]
        public IEnumerable<Snapshot> Tickers { get; set; }
    }
}
