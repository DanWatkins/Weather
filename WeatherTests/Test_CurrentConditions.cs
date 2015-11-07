//=============================================================================|
// Copyright (C) 2015 Dan Watkins
// This file is licensed under the MIT License.
//=============================================================================|

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Weather;

namespace WeatherTests
{
    [TestClass]
    public class Test_CurrentConditions
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cc = new CurrentConditions(new Mock_CurrentConditionsProvider());

            Assert.AreEqual("Hugo", cc.City);
            Assert.AreEqual("MN", cc.State);
            Assert.AreEqual(281, cc.Elevation);
            Assert.AreEqual("Fri, 06 Nov 2015 18:45:43 -0600", cc.ObservationTime);

            Assert.AreEqual("Overcast", cc.Brief);
            Assert.AreEqual(5.4, cc.Temperature);
            Assert.AreEqual(0.81, cc.RelativeHumidity);
            Assert.AreEqual(0.0, cc.WindSpeed);
            Assert.AreEqual(4.8, cc.WindGustSpeed);
            Assert.AreEqual(16.1, cc.Visibility);
        }
    }
}
