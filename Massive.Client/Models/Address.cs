using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Massive.Client.Models
{
    [ExcludeFromCodeCoverage]
    public class Address
    {
        /// <summary>
        /// The first line of the company's headquarters address.
        /// </summary>
        [JsonPropertyName("address1")]
        public string Address1 { get; set; }

        /// <summary>
        /// The city of the company's headquarters address.
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// The postal code of the company's headquarters address.
        /// </summary>
        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }

        /// <summary>
        /// The state of the company's headquarters address.
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
