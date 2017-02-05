﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace Weather
{
    public class Conditions
    {
        public decimal Temperature { get; set; }

        public decimal WindSpeed { get; set; }

        public string WindDirection { get; set; }

        public Location Location { get; set; }

        public string Description { get; set; }
        public string Humidity { get; set; }
        public decimal Visibility { get; set; }

        public static Conditions ForZipCode(IWeatherService weatherService, string zipCode)
        {
            try
            {
                var weatherInfo = new Conditions();
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
            string xmlBuffer = weatherService.GetCurrentConditionsForZipCode(zipCode);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlBuffer);

            var node_response = xmlDocument.FirstChild;
            var node_error = node_response["error"];

            if (node_error != null)
            {
                var map = new Dictionary<string, string>();
                map.Add("No cities match your search query",
                    "The specified zip code is not valid.");
                map.Add("this key does not exist",
                    "The API key for the weather service is not valid.");

#warning This is unsafe access to the description element
                throw new WeatherException(map[node_error["description"].InnerText]);
            }

            FillFromCurrentObservationXMLElement(node_response["current_observation"]);
        }

        private void FillFromCurrentObservationXMLElement(XmlElement node_currentObservation)
        {
            Temperature = decimal.Parse(node_currentObservation["temp_c"].InnerText);
            WindSpeed = decimal.Parse(node_currentObservation["wind_kph"].InnerText);
            WindDirection = node_currentObservation["wind_dir"].InnerText;
            Description = node_currentObservation["weather"].InnerText;

            var node_displayLocation = node_currentObservation["display_location"];
            {
                Location = new Location();
                Location.City = node_displayLocation["city"].InnerText;
                Location.State = node_displayLocation["state"].InnerText;
                Location.Country = node_displayLocation["country"].InnerText;
                Location.Elevation = (int)double.Parse(node_displayLocation["elevation"].InnerText);
            }
        }
    }
}