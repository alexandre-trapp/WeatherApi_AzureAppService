using System;
using RestSharp;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherApiService.Models;
using WeatherApiService.Services;

namespace WeatherApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        public IRestClient _restClient { get; set; } = new RestClient();

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        ///<summary>
        ///Code from city to request api Weather
        ///</summary>
        [HttpGet("{idCity}")]
        public async Task<IActionResult> Get([FromRoute] string idCity)
        {
            var request = new string[1] { idCity };
            return await ProcessRequestWeather(request);
        }

        ///<summary>
        ///Codes from cities to request api Weather, separated with ',' or ';'
        ///</summary>
        [HttpGet]
        [Route("cities/{cities}")]
        public async Task<IActionResult> GetCities([FromRoute] string cities)
        {
            string[] citiesArr = GetCitiesSplitedWithSeparator(cities);
            return await ProcessRequestWeather(citiesArr);
        }

        private static string[] GetCitiesSplitedWithSeparator(string cities)
        {
            if (string.IsNullOrEmpty(cities))
                return new string[0];

            var citiesArr = cities.Split(',');
            if (citiesArr.Length == 0)
                citiesArr = cities.Split(';');

            return citiesArr;
        }

        private async Task<IActionResult> ProcessRequestWeather(string[] cities)
        {
            if (!ContainCitiesReques(cities))
            {
                var responseNotFound = new ResponseWeather
                {
                    MessageResponse = "não informado cidades válidas no request"
                };

                return NotFound(JsonConvert.SerializeObject(responseNotFound));
            };

            var response = await ConsumeOpenWeatherApi(cities);
            return Ok(JsonConvert.SerializeObject(response));
        }

        private async Task<ResponseWeather> ConsumeOpenWeatherApi(string[] cities)
        {
            const string token = "eb8b1a9405e659b2ffc78f0a520b1a46";
            var responseList = new ResponseWeather();

            var sbLog = new StringBuilder();

            foreach (var city in cities)
            {
                _restClient.BaseUrl = new Uri($"http://api.openweathermap.org/data/2.5/forecast?id={city}&APPID={token}");
                var request = new RestRequest(Method.GET);

                var resp = await _restClient.ExecuteGetAsync<IRestResponse>(request);

                if (resp == null)
                {
                    sbLog.AppendLine($"response is null; Request idCity: {city}");
                    continue;
                }
                
                Console.WriteLine(resp.Content);
                if (string.IsNullOrEmpty(resp.Content))
                    sbLog.AppendLine($"Content is empty; Request idCity: {city}");

                var weather = JsonConvert.DeserializeObject<WeatherForecast>(resp.Content);
                responseList.WeathersList.Add(weather);
            }

            responseList.MessageResponse = (string.IsNullOrEmpty(sbLog.ToString()) ? "Success" : sbLog.ToString());

            return responseList;
        }

        private bool ContainCitiesReques(string[] cities)
        {
            return cities != null && cities.Length > 0;
        }
    }
}
