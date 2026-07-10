using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Massive.Client.Models
{
    [ExcludeFromCodeCoverage]
    public class TickerDetails
    {
        /// <summary>
        /// Whether or not the asset is actively traded. False means the asset has been delisted.
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonPropertyName("branding")]
        public Branding Branding { get; set; }

        /// <summary>
        /// The CIK number for this ticker. Find more information here: https://en.wikipedia.org/wiki/Central_Index_Key
        /// </summary>
        [JsonPropertyName("cik")]
        public string Cik { get; set; }

        /// <summary>
        /// The market type of the asset.
        /// </summary>
        [JsonPropertyName("market")]
        public string Market { get; set; }

        /// <summary>
        /// The most recent close price of the ticker multiplied by weighted outstanding shares.
        /// </summary>
        [JsonPropertyName("market_cap")]
        public double MarketCap { get; set; }

        /// <summary>
        /// The shares outstanding calculated assuming all shares of other
        /// share classes are converted to this share class.
        /// </summary>
        [JsonPropertyName("weighted_shares_outstanding")]
        public long Float { get; set; }

        /// <summary>
        /// The exchange symbol that this item is traded under.
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        /// <summary>
        /// The name of the asset. For stocks/equities this will be the companies registered name.
        /// For crypto/fx this will be the name of the currency or coin pair.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The ISO code of the primary listing exchange for this asset.
        /// </summary>
        [JsonPropertyName("primary_exchange")]
        public string PrimaryExchange { get; set; }

        /// <summary>
        /// The type of the asset. Find the types that we support via our
        /// Ticker Types API: https://massive.com/docs/rest/stocks/tickers/types.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
