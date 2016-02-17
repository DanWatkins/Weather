using System.Xml;

namespace Weather
{
    public class WeatherInfo
    {
        public double Temperature { get; set; }

        public double WindSpeed { get; set; }

        readonly IWeatherService _weatherService;

        public WeatherInfo(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public void UpdateForZipCode(string zipCode)
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