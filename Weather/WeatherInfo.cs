using System;
using System.Net;
using System.Xml;

namespace Weather
{
    public class WeatherException : Exception
    {
        public WeatherException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }

    public class WeatherInfo
    {
        public double Temperature { get; set; }

        public double WindSpeed { get; set; }

        readonly IWeatherService _weatherService;

        private WeatherInfo(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public static WeatherInfo ForZipCode(IWeatherService weatherService, string zipCode)
        {
            try
            {
                var weatherInfo = new WeatherInfo(weatherService);
                weatherInfo.UpdateForZipCode(zipCode);

                return weatherInfo;
            }
            catch (WebException webException)
            {
                throw new WeatherException("There was a network error while connecting to the weather service.", webException);
            }
            catch (XmlException xmlException)
            {
                throw new WeatherException("There was an xml error while parsing weather data fromt he weather service.", xmlException);
            }
        }

        private void UpdateForZipCode(string zipCode)
        {
            string xmlBuffer = _weatherService.GetWeatherForZipCode(zipCode);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlBuffer);

            var node_response = xmlDocument.FirstChild;
            var node_currentObservation = node_response["current_observation"];
            {
                Temperature = double.Parse(node_currentObservation["temp_c"].InnerText);
                WindSpeed = double.Parse(node_currentObservation["wind_kph"].InnerText);
            }
        }
    }
}