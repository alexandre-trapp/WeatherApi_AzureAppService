using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WeatherApiService.Models
{
    public class WeatherForecast
    {
        [JsonProperty("cod")]
        public long Cod { get; set; }

        [JsonProperty("message")]
        public long Message { get; set; }

        [JsonProperty("cnt")]
        public long Cnt { get; set; }

        [JsonProperty("list")]
        public ListWeathers[] List { get; set; }

        [JsonProperty("city")]
        public City City { get; set; }
    }

    public class ListWeathers
    {
        [JsonProperty("dt")]
        public long Dt { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }

        [JsonProperty("dt_txt")]
        public string DtTxt { get; set; }

        [JsonProperty("sys")]
        public SystemData Sys { get; set; }

        [JsonProperty("main")]
        public AtmosphericData Main { get; set; }

        [JsonProperty("weather")]
        public Weather[] Weather { get; set; }

        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; }
    }

    public class Clouds
    {
        [JsonProperty("all")]
        public long All { get; set; }
    }

    public class AtmosphericData
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("feels_like")]
        public double Feels_Like { get; set; }

        [JsonProperty("temp_min")]
        public double TempMin { get; set; }

        [JsonProperty("temp_max")]
        public double TempMax { get; set; }

        [JsonProperty("pressure")]
        public long Pressure { get; set; }

        [JsonProperty("sea_level")]
        public long SeaLevel { get; set; }

        [JsonProperty("grnd_level")]
        public long GrndLevel { get; set; }

        [JsonProperty("humidity")]
        public long Humidity { get; set; }

        [JsonProperty("temp_kf")]
        public double TempKf { get; set; }
    }

    public partial class SystemData
    {
        [JsonProperty("pod")]
        public string Pod { get; set; }
    }

    public class Weather
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("main")]
        public string Main { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    public partial class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("deg")]
        public long Deg { get; set; }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
