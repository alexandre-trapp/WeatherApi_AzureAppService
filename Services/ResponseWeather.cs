using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using WeatherApiService.Models;

namespace WeatherApiService.Services
{
    public class ResponseWeather
    {
        [JsonProperty("weathersList")]
        public List<WeatherForecast> WeathersList { get; set; } = new List<WeatherForecast>();

        [JsonProperty("messageResponse")]
        public string MessageResponse { get; set; }
    }
}
