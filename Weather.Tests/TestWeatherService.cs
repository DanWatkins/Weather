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
            var expectedWeatherInfo = new CurrentConditions
            {
                Temperature = 16.2,
                WindSpeed = 0.0,
                Location = new Location
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
            var expectedWeatherInfo = new CurrentConditions
            {
                Temperature = 8.8,
                WindSpeed = 14.5,
                Location = new Location
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
        public void GetWeatherForZipCode_ErrorNocities()
        {
            var weatherService = new Mock<IWeatherService>();
            weatherService.Setup(f => f.GetCurrentConditionsForZipCode("00000"))
                .Returns(Properties.Resources.CurrentConditions_ErrorNoCities);

            TestGetWeatherForZipCodeErrorNoCities(weatherService.Object);
        }

        [Test]
        public void GetWeatherForZipCode_ErrorInvalidAPIKey()
        {
            var weatherService = new Mock<IWeatherService>();
            weatherService.Setup(f => f.GetCurrentConditionsForZipCode("00000"))
                .Returns(Properties.Resources.CurrentConditions_ErrorInvalidAPIKey);

            TestGetWeatherForZipCodeErrorInvalidAPIKey(weatherService.Object);
        }

        [Test]
        [Category(TestCategory.Integration)]
        public void GetLiveWeatherForZipCode_58102()
        {
            var weatherInfo = CurrentConditions.ForZipCode(CreateLiveWeatherService(), "58102");

            Assert.AreEqual("Fargo", weatherInfo.Location.City);
            Assert.AreEqual("ND", weatherInfo.Location.State);
            Assert.AreEqual("US", weatherInfo.Location.Country);
            Assert.AreEqual(271, weatherInfo.Location.Elevation);

            Assert.IsTrue(-200 <= weatherInfo.Temperature && weatherInfo.Temperature <= 200);
        }

        [Test]
        [Category(TestCategory.Integration)]
        public void GetLiveWeatherForZipCode_ErrorNoCities()
        {
            TestGetWeatherForZipCodeErrorNoCities(CreateLiveWeatherService());
        }

        [Test]
        [Category(TestCategory.Integration)]
        public void GetLiveWeatherForZipCode_ErrorInvalidAPIKey()
        {
            TestGetWeatherForZipCodeErrorInvalidAPIKey(new WeatherService("NotARealKey_adkfha4adg924t8a4o78an64d7"));
        }

        #endregion

        private void TestGetWeatherForZipCode(string zipCode, string xmlWeatherData, CurrentConditions exp)
        {
            var weatherService = new Mock<IWeatherService>();
            weatherService.Setup(f => f.GetCurrentConditionsForZipCode(zipCode))
                .Returns(xmlWeatherData);

            var weatherInfo = CurrentConditions.ForZipCode(weatherService.Object, zipCode);

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
            weatherService.Setup(f => f.GetCurrentConditionsForZipCode("80001"))
                .Throws(new TException());
            CurrentConditions weatherInfo = null;

            var weatherException = Assert.Throws<WeatherException>(delegate
            {
                weatherInfo = CurrentConditions.ForZipCode(weatherService.Object, "80001");
            });

            Assert.AreEqual(message, weatherException.Message);
            Assert.IsNotNull(weatherException.InnerException);
            Assert.IsTrue(weatherException.InnerException is TException);
        }

        private void TestGetWeatherForZipCodeErrorNoCities(IWeatherService weatherService)
        {
            var weatherException = Assert.Throws<WeatherException>(delegate
            {
                CurrentConditions.ForZipCode(weatherService, "00000");
            });

            Assert.AreEqual("The specified zip code is not valid.", weatherException.Message);
            Assert.IsNull(weatherException.InnerException);
        }

        private void TestGetWeatherForZipCodeErrorInvalidAPIKey(IWeatherService weatherService)
        {
            var weatherException = Assert.Throws<WeatherException>(delegate
            {
                CurrentConditions.ForZipCode(weatherService, "00000");
            });

            Assert.AreEqual("The API key for the weather service is not valid.", weatherException.Message);
            Assert.IsNull(weatherException.InnerException);
        }

        private WeatherService CreateLiveWeatherService()
        {
            return new WeatherService(Properties.Resources.WundergroundApiKey.Trim());
        }
    }
}