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
        [Test]
        public void GetWeatherforZipCode_34101()
        {
            var weatherService = new Mock<IWeatherService>();
            weatherService.Setup(f => f.GetWeatherForZipCode("34101"))
                .Returns(Properties.Resources.CurrentConditions_Zip_34101);

            var weatherInfo = WeatherInfo.ForZipCode(weatherService.Object, "34101");

            Assert.AreEqual(16.2, weatherInfo.Temperature);
            Assert.AreEqual(0.0, weatherInfo.WindSpeed);
        }

        [Test]
        public void GetWeatherforZipCode_55038()
        {
            var weatherService = new Mock<IWeatherService>();
            weatherService.Setup(f => f.GetWeatherForZipCode("55038"))
                .Returns(Properties.Resources.CurrentConditions_Zip_55038);

            var weatherInfo = WeatherInfo.ForZipCode(weatherService.Object, "55038");

            Assert.AreEqual(8.8, weatherInfo.Temperature);
            Assert.AreEqual(14.5, weatherInfo.WindSpeed);
        }

        [Test]
        public void GetWeatherForZipCode_WebException()
        {
            TestException<WebException>("There was a network error while connecting to the weather service.");
        }

        [Test]
        public void GetWeatherForZipCode_XmlException()
        {
            TestException<XmlException>("There was an xml error while parsing weather data fromt he weather service.");
        }

        private void TestException<TException>(string message)
            where TException : Exception, new()
        {
            var weatherService = new Mock<IWeatherService>();
            weatherService.Setup(f => f.GetWeatherForZipCode("80001"))
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