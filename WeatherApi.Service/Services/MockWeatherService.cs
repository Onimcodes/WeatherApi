using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApi.Repository.MockWeatherRepo;

namespace WeatherApi.Service.Services
{
    public class MockWeatherService : IMockWeatherService
    {
        private readonly IMockWeatherRepo mockWeatherRepo;

        public MockWeatherService(IMockWeatherRepo mockWeatherRepo)
        {
            this.mockWeatherRepo = mockWeatherRepo;
        }
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            var weatherforecast = mockWeatherRepo.Get();
            return weatherforecast; 
        }
    }


    public interface IMockWeatherService
    {
        IEnumerable<WeatherForecast> GetWeatherForecast();
    }

   
}
