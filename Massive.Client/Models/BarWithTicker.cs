using System.Text.Json.Serialization;

namespace Massive.Client.Models
{
    [JsonConverter(typeof(BarWithTickerConverter))]
    public class BarWithTicker : Bar
    {
        public string Ticker { get; set; }
    }
}
