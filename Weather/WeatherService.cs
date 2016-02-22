using System;
using System.Configuration;
using System.Net;

namespace Weather
{
    public interface IWeatherService
    {
        string GetConditionsForZipCode(string zipCode);
    }

    public class WeatherService : IWeatherService
    {
        readonly string _apiKey;

        public WeatherService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public string GetConditionsForZipCode(string zipCode)
        {
            string url = "http://api.wunderground.com/api/" + _apiKey + "/conditions/q/" + zipCode + ".xml";

            return new WebClient().DownloadString(url);
        }
    }
}