using System.Net;

namespace Weather
{
    public interface IWeatherService
    {
        string GetCurrentConditionsForZipCode(string zipCode);
    }

    public class WeatherService : IWeatherService
    {
        readonly string _apiKey;

        public WeatherService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public string GetCurrentConditionsForZipCode(string zipCode)
        {
            string url = "http://api.wunderground.com/api/" + _apiKey + "/conditions/q/" + zipCode + ".xml";

            return new WebClient().DownloadString(url);
        }
    }
}