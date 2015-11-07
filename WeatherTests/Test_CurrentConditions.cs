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
        class Mock_CurrentConditionsProvider1 : ICurrentConditionsProvider
        {
            string ICurrentConditionsProvider.QueryCurrentConditions()
            {
                return Properties.Resources.CurrentConditions_55038;
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            var cc = new CurrentConditions(new Mock_CurrentConditionsProvider1());

            Assert.AreEqual("Hugo", cc.City);
            Assert.AreEqual("MN", cc.State);
            Assert.AreEqual(284, cc.Elevation);
            Assert.AreEqual("Sat, 07 Nov 2015 13:21:01 -0600", cc.ObservationTime);

            Assert.AreEqual("Clear", cc.Brief);
            Assert.AreEqual(8.8, cc.Temperature);
            Assert.AreEqual(0.57, cc.RelativeHumidity);
            Assert.AreEqual(14.5, cc.WindSpeed);
            Assert.AreEqual(20.9, cc.WindGustSpeed);
            Assert.AreEqual(16.1, cc.Visibility);
        }
    }
}