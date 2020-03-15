using Newtonsoft.Json;

namespace WeatherApiService.Models
{
    public class City
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("timezone")]
        public long Timezone { get; set; }

        [JsonProperty("sunrise")]
        public long Sunrise { get; set; }

        [JsonProperty("sunset")]
        public long Sunset { get; set; }

        public CoordinatesLocalization Coord { get; set; }
    }

    [JsonObject("coord")]
    public class CoordinatesLocalization
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; } 
    }

    public static class Serialize
    {
        public static string ToJson(this City self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
}

