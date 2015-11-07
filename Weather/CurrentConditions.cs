using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    public class CurrentConditions
    {
        /**
         * City where the conditions were observed.
         */
        public string City { get; set; } = "";

        /**
         * State where the conditions were observed.
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
        public double Brief { get; set; }

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

        private ICurrentConditionsProvider _currentConditionsProvider;

        private CurrentConditions() { }

        public CurrentConditions(ICurrentConditionsProvider currentConditionsProvider)
        {
            
        }
    }
}