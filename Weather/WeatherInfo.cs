using System;
using System.Net;
using System.Xml;

namespace Weather
{
    public class WeatherException : Exception
    {
        public WeatherException(string message) :
            base(message)
        {
        }

        public WeatherException(string message, Exception innerException) :
            base(message, innerException)
        {
        }
    }

    public class WeatherInfo
    {
        public double Temperature { get; set; }

        public double WindSpeed { get; set; }

        public WeatherLocation Location { get; set; }

        public WeatherInfo()
        {
        }

        public static WeatherInfo ForZipCode(IWeatherService weatherService, string zipCode)
        {
            try
            {
                var weatherInfo = new WeatherInfo();
                weatherInfo.UpdateForZipCode(weatherService, zipCode);

                return weatherInfo;
            }
            catch (WebException webException)
            {
                throw new WeatherException("There was a network error while connecting to the weather service.", webException);
            }
            catch (XmlException xmlException)
            {
                throw new WeatherException("There was an xml error while parsing weather data from the weather service.", xmlException);
            }
        }

        private void UpdateForZipCode(IWeatherService weatherService, string zipCode)
        {
            string xmlBuffer = weatherService.GetConditionsForZipCode(zipCode);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlBuffer);

            var node_response = xmlDocument.FirstChild;
            var node_error = node_response["error"];

            if (node_error != null)
            {
                throw new WeatherException(node_error["description"].InnerText);
            }

            var node_currentObservation = node_response["current_observation"];
            {
                Temperature = double.Parse(node_currentObservation["temp_c"].InnerText);
                WindSpeed = double.Parse(node_currentObservation["wind_kph"].InnerText);

                var node_displayLocation = node_currentObservation["display_location"];
                {
                    Location = new WeatherLocation();
                    Location.City = node_displayLocation["city"].InnerText;
                    Location.State = node_displayLocation["state"].InnerText;
                    Location.Country = node_displayLocation["country"].InnerText;
                    Location.Elevation = (int)double.Parse(node_displayLocation["elevation"].InnerText); 
                }
            }
        }
    }
}