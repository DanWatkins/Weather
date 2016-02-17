using Moq;
using NUnit.Framework;

namespace Weather.Tests
{
    [TestFixture]
	public class TestWeatherService
	{
		[Test]
		public void QueryWeatherService1()
		{
			var weatherService = new Mock<IWeatherService>();
			weatherService.Setup(f => f.GetWeatherForZipCode("55038"))
				.Returns(Properties.Resources.CurrentConditions_Zip_55038);

			var weatherInfo = new WeatherInfo(weatherService.Object);
			weatherInfo.UpdateForZipCode("55038");

			Assert.AreEqual(8.8, weatherInfo.Temperature);
			Assert.AreEqual(14.5, weatherInfo.WindSpeed);
		}

		[Test]
		public void QueryWeatherService2()
		{
			var weatherService = new Mock<IWeatherService>();
			weatherService.Setup(f => f.GetWeatherForZipCode("34101"))
				.Returns(Properties.Resources.CurrentConditions_Zip_34101);

			var weatherInfo = new WeatherInfo(weatherService.Object);
			weatherInfo.UpdateForZipCode("34101");

			Assert.AreEqual(16.2, weatherInfo.Temperature);
			Assert.AreEqual(0.0, weatherInfo.WindSpeed);
		}
	}
}