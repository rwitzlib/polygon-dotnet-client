using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Massive.Client.Requests
{
    /// <summary>
    /// API Reference: https://massive.com/docs/rest/stocks/tickers
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MassiveGetTickersRequest
    {
        /// <summary>
        /// Specify a ticker symbol. Defaults to empty string which queries all tickers.
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        /// <summary>
        /// Specify the type of the tickers. Find the types that we support via our Ticker Types API.
        /// Defaults to empty string which queries all types.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Filter by market type. By default all markets are included.
        /// </summary>
        [JsonPropertyName("market")]
        public string Market { get; set; }

        /// <summary>
        /// Specify the primary exchange of the asset in the ISO code format. Find more
        /// information about the ISO codes at the ISO org website.
        /// Defaults to empty string which queries all exchanges.
        /// </summary>
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        /// <summary>
        /// Specify the CUSIP code of the asset you want to search for. Find more information about CUSIP codes at their website.
        /// Defaults to empty string which queries all CUSIPs.
        ///
        ///     Note: Although you can query by CUSIP, due to legal reasons we do not return the CUSIP in the response.
        /// </summary>
        [JsonPropertyName("cusip")]
        public string Cusip { get; set; }

        /// <summary>
        /// Specify the CIK of the asset you want to search for. Find more information
        /// about CIK codes at their website. Defaults to empty string which queries all CIKs.
        /// </summary>
        [JsonPropertyName("cik")]
        public string Cik { get; set; }

        /// <summary>
        /// Specify a point in time to retrieve tickers available on that date. Defaults to the most recent available date.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Search for terms within the ticker and/or company name.
        /// </summary>
        [JsonPropertyName("search")]
        public string Search { get; set; }

        /// <summary>
        /// Specify if the tickers returned should be actively traded on the queried date. Default is true.
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; } = true;

        /// <summary>
        /// Number of tickers that should be returned.
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; } = 1000;

        /// <summary>
        /// Order results based on the sort field.
        /// </summary>
        [JsonPropertyName("order")]
        public string Order { get; set; } = "asc";

        /// <summary>
        /// Sort field used for ordering.
        /// </summary>
        [JsonPropertyName("sort")]
        public string Sort { get; set; }
    }
}
