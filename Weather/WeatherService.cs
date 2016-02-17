using System;

namespace Weather
{
    public interface IWeatherService
    {
        string GetWeatherForZipCode(string zipCode);
    }

    public class WeatherService : IWeatherService
    {
        public string GetWeatherForZipCode(string zipCode)
        {
            throw new NotImplementedException();
        }
    }
}