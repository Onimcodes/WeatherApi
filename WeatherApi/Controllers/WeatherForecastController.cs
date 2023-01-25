using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Service.Services;

namespace WeatherApi.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherService weatherservice;
        private readonly IMockWeatherService mockWeatherService;

        public WeatherForecastController(IWeatherService weatherservice, IMockWeatherService mockWeatherService)
        {
            this.weatherservice = weatherservice;
            this.mockWeatherService = mockWeatherService;
        }


        //To Test this endpoint get api key from https://home.openweathermap.org/
        //and put it in appsettings Openweatherclient section

        [HttpGet]
        [Route(("GetCurrentWeatherForCity"))]
        public async Task<IActionResult> GetWeatherForCity(string city)
        {
            var weather =  await weatherservice.GetCurrentWeatherForCity(city);  
            
            return Ok(weather);
        }

        //To test this endpoint , you need to login as admin and use
        //Data is coming from mockweatherservice
        //Admin email example : Abrahammicheal55@gmail.com
        //Admin password: Coding@1234?
        

        [HttpGet]
        [Authorize(Roles = "admin")]
        [Route ("GetWeatherForecast")]
        public IActionResult GetWeatherForecast()
        {
            var weatherForecast = mockWeatherService.GetWeatherForecast();
            return Ok(weatherForecast);
        }

    }
}