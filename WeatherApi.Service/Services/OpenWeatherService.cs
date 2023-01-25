using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WeatherApi.Service.Models;

namespace WeatherApi.Service.Services
{
    public class OpenWeatherService : IWeatherService
    {

        private readonly HttpClient httpClient;
        private readonly IConfiguration config;

        public OpenWeatherService(HttpClient httpClient, IConfiguration config)
        {
            this.httpClient = httpClient;
            this.config = config;
        }
        public async Task<WeatherResponse?> GetCurrentWeatherForCity(string city)
        {
            return await httpClient.GetFromJsonAsync<WeatherResponse>(
                $"weather?q={city}&appid={config["OpenWeatherClient:ApiKey"]}");
        }
    }

    public interface IWeatherService
    {
        Task<WeatherResponse?> GetCurrentWeatherForCity(string city);    
    }


    
}
