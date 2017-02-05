namespace Weather.Client.Desktop.Windows.ViewModel
{
    internal class ConditionsViewModel
    {
        private Conditions _conditions;

        public string LocationName => $"{_conditions.Location.City}, {_conditions.Location.State}";

        public string GeneralSummary => _conditions.Description;

        public string WindSummary => $"Wind {_conditions.WindDirection} {_conditions.WindSpeed} mph";

        public string HumiditySummary => $"Humidity {_conditions.Humidity}";

        public string VisibilitySummary => $"Visibility {_conditions.Visibility} miles";

        public ConditionsViewModel()
        {
            _conditions = new Conditions
            {
                Location = new Location
                {
                    City = "Hugo",
                    Country = "US",
                    Elevation = 932,
                    State = "MN"
                },
                Temperature = 19.0m,
                WindSpeed = 16.3m,
                WindDirection = "NE",
                Humidity = "65%",
                Visibility = 7.0m,
                Description = "Partly Cloudy"
            };
        }
    }
}