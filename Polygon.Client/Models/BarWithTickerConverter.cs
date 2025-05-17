using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Polygon.Client.Models
{
    public class BarWithTickerConverter : JsonConverter<BarWithTicker>
    {
        public override BarWithTicker Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var result = new BarWithTicker();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "T":
                        result.Ticker = reader.GetString();
                        break;
                    case "v":
                        result.Volume = reader.GetSingle();
                        break;
                    case "vw":
                        result.Vwap = reader.GetSingle();
                        break;
                    case "o":
                        result.Open = reader.GetSingle();
                        break;
                    case "c":
                        result.Close = reader.GetSingle();
                        break;
                    case "h":
                        result.High = reader.GetSingle();
                        break;
                    case "l":
                        result.Low = reader.GetSingle();
                        break;
                    case "t":
                        result.Timestamp = reader.GetInt64();
                        break;
                    case "n":
                        result.TransactionCount = reader.GetInt32();
                        break;
                }
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, BarWithTicker value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("T", value.Ticker);
            writer.WriteNumber("v", value.Volume);
            writer.WriteNumber("vw", value.Vwap);
            writer.WriteNumber("o", value.Open);
            writer.WriteNumber("c", value.Close);
            writer.WriteNumber("h", value.High);
            writer.WriteNumber("l", value.Low);
            writer.WriteNumber("t", value.Timestamp);
            writer.WriteNumber("n", value.TransactionCount);
            writer.WriteEndObject();
        }
    }
} 