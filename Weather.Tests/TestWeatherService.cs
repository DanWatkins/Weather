using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Xml;

namespace Weather.Tests
{
    [TestFixture]
    public class TestWeatherService
    {
        #region Tests

        [Test]
        public void GetWeatherforZipCode_34101()
        {
            var expectedWeatherInfo = new WeatherInfo
            {
                Temperature = 16.2,
                WindSpeed = 0.0,
                Location = new WeatherLocation
                {
                    City = "Naples",
                    State = "FL",
                    Country = "US",
                    Elevation = 5
                }
            };

            TestGetWeatherForZipCode("34101", Properties.Resources.CurrentConditions_Zip_34101, expectedWeatherInfo);
        }

        [Test]
        public void GetWeatherForZipCode_55038()
        {
            var expectedWeatherInfo = new WeatherInfo
            {
                Temperature = 8.8,
                WindSpeed = 14.5,
                Location = new WeatherLocation
                {
                    City = "Hugo",
                    State = "MN",
                    Country = "US",
                    Elevation = 284
                }
            };

            TestGetWeatherForZipCode("55038", Properties.Resources.CurrentConditions_Zip_55038, expectedWeatherInfo);
        }

        [Test]
        public void GetWeatherForZipCode_WebException()
        {
            TestWeatherException<WebException>("There was a network error while connecting to the weather service.");
        }

        [Test]
        public void GetWeatherForZipCode_XmlException()
        {
            TestWeatherException<XmlException>("There was an xml error while parsing weather data from the weather service.");
        }

        [Test]
        [Category(TestCategory.Integration)]
        public void GetLiveWeatherForZipCode_58102()
        {
            string apiKey = Weather.Properties.Resources.WeatherUndergroundAPIKey.Trim();
            var weatherInfo = WeatherInfo.ForZipCode(new WeatherService(apiKey), "58102");

            Assert.AreEqual("Fargo", weatherInfo.Location.City);
            Assert.AreEqual("ND", weatherInfo.Location.State);
            Assert.AreEqual("US", weatherInfo.Location.Country);
            Assert.AreEqual(271, weatherInfo.Location.Elevation);

            Assert.IsTrue(-200 <= weatherInfo.Temperature && weatherInfo.Temperature <= 200);
        }

        #endregion

        private void TestGetWeatherForZipCode(string zipCode, string xmlWeatherData, WeatherInfo exp)
        {
            var weatherService = new Mock<IWeatherService>();
            weatherService.Setup(f => f.GetConditionsForZipCode(zipCode))
                .Returns(xmlWeatherData);

            var weatherInfo = WeatherInfo.ForZipCode(weatherService.Object, zipCode);

            Assert.AreEqual(exp.Temperature, weatherInfo.Temperature);
            Assert.AreEqual(exp.WindSpeed, weatherInfo.WindSpeed);

            {
                Assert.AreEqual(exp.Location.City, weatherInfo.Location.City);
                Assert.AreEqual(exp.Location.State, weatherInfo.Location.State);
                Assert.AreEqual(exp.Location.Country, weatherInfo.Location.Country);
                Assert.AreEqual(exp.Location.Elevation, weatherInfo.Location.Elevation);
            }
        }

        private void TestWeatherException<TException>(string message)
            where TException : Exception, new()
        {
            var weatherService = new Mock<IWeatherService>();
            weatherService.Setup(f => f.GetConditionsForZipCode("80001"))
                .Throws(new TException());
            WeatherInfo weatherInfo = null;

            var weatherException = Assert.Throws<WeatherException>(delegate
            {
                weatherInfo = WeatherInfo.ForZipCode(weatherService.Object, "80001");
            });

            Assert.AreEqual(message, weatherException.Message);
            Assert.IsNotNull(weatherException.InnerException);
            Assert.IsTrue(weatherException.InnerException is TException);
        }
    }
}