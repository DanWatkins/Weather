//=============================================================================|
// Copyright (C) 2015 Dan Watkins
// This file is licensed under the MIT License.
//=============================================================================|

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Weather
{
    public class CurrentConditions
    {
        /**
         * City where the conditions represent.
         */
        public string City { get; set; } = "";

        /**
         * State where the conditions represent.
         */
        public string State { get; set; } = "";

        /**
         * Measured in meters.
         */
        public int Elevation { get; set; }

        /**
         * When the conditions where observed.
         */
        public string ObservationTime { get; set; } = "";

        /**
         * A brief description for the type of weather.
         */
        public String Brief { get; set; }

        /**
         * Measured in degrees celsius.
         */
        public double Temperature { get; set; }

        /**
         * Measured as a proportion (0.0 to 1.0).
         */
        public double RelativeHumidity { get; set; }

        /**
         * Measured in kph.
         */
        public double WindSpeed { get; set; }

        /**
         * Measured in kph.
         */
        public double WindGustSpeed { get; set; }

        /**
         * Measured in km.
         */
        public double Visibility { get; set; }

        /**
         * Error string describing a problem. Null if no error occured.
         */
         public string Error { get; set; }

        public CurrentConditions(ICurrentConditionsProvider currentConditionsProvider)
        {
            ParseInputXml(currentConditionsProvider.QueryCurrentConditions());
        }

        private void ParseInputXml(string inputXml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(inputXml);
            var node_response = xmlDocument.FirstChild;

            var node_error = node_response["error"];

            if (node_error != null)
            {
                Error = node_error["description"].InnerText;
                return;
            }

            var node_currentObservation = node_response["current_observation"];
            {
                var node_displayLocation = node_currentObservation["display_location"];
                {
                    City = node_displayLocation["city"].InnerText;
                    State = node_displayLocation["state"].InnerText;
                    Elevation = (int)double.Parse(node_displayLocation["elevation"].InnerText);
                }

                ObservationTime = node_currentObservation["observation_time_rfc822"].InnerText;
                Brief = node_currentObservation["weather"].InnerText;
                Temperature = double.Parse(node_currentObservation["temp_c"].InnerText);
                RelativeHumidity = double.Parse(node_currentObservation["relative_humidity"].InnerText.Replace("%", "")) / 100.0;
                WindSpeed = double.Parse(node_currentObservation["wind_kph"].InnerText);
                WindGustSpeed = double.Parse(node_currentObservation["wind_gust_kph"].InnerText);
                Visibility = double.Parse(node_currentObservation["visibility_km"].InnerText);
            }
        }
    }
}