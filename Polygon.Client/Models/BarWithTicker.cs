using Polygon.Client.Models;
using System.Text.Json.Serialization;

namespace Polygon.Client.Models
{
    [JsonConverter(typeof(BarWithTickerConverter))]
    public class BarWithTicker : Bar
    {
        public string Ticker { get; set; }
    }
}
