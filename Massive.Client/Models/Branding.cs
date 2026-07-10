using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Massive.Client.Models
{
    [ExcludeFromCodeCoverage]
    public class Branding
    {
        /// <summary>
        /// A link to this ticker's company's icon. Icon's are generally smaller,
        /// square images that represent the company at a glance.
        /// </summary>
        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; }

        /// <summary>
        /// A link to this ticker's company's logo.
        /// </summary>
        [JsonPropertyName("logo_url")]
        public string LogoUrl { get; set; }
    }
}
