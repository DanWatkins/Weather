//=============================================================================|
// Copyright (C) 2015 Dan Watkins
// This file is licensed under the MIT License.
//=============================================================================|

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Weather;
using System.IO;
using System.Net;

namespace WeatherTests
{
    [TestClass]
    public class Test_CurrentConditions
    {
        private static string _apiKey = "";

        [ClassInitialize]
        static public void ClassInitialize(TestContext testContext)
        {
            var currentConditionsProvider = new CurrentConditionsProvider();

            var apiKeyForm = new APIKeyForm();
            apiKeyForm.APIKey = new APIKey();

            string tempFile = Path.GetTempFileName();
            apiKeyForm.APIKey.LinkCacheFile(tempFile);

            if (apiKeyForm.APIKey.Value == null)
            {
                apiKeyForm.ShowDialog();
            }

            _apiKey = apiKeyForm.APIKey.Value;
            Assert.IsNotNull(_apiKey);
        }

        class Mock_CurrentConditionsProvider1 : CurrentConditionsProviderBase
        {
            public override QueryResult QueryCurrentConditions()
            {
                var queryResult = new QueryResult();
                queryResult.XmlData = Properties.Resources.CurrentConditions_55038;

                return queryResult;
            }
        }

        [TestMethod]
        public void ParseNormalCurrentConditionsData()
        {
            var cc = new CurrentConditions(new Mock_CurrentConditionsProvider1());

            Assert.IsNull(cc.Error);

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

        class Mock_CurrentConditionsProvider2 : CurrentConditionsProviderBase
        {
            public override QueryResult QueryCurrentConditions()
            {
                var queryResult = new QueryResult();
                queryResult.XmlData = Properties.Resources.CurrentConditions_10000;

                return queryResult;
            }
        }

        [TestMethod]
        public void ParseUnknownZipCodeCurrentConditionsData()
        {
            var cc = new CurrentConditions(new Mock_CurrentConditionsProvider2());

            Assert.IsNotNull(cc.Error);
            Assert.AreEqual("No cities match your search query", cc.Error);
        }

        class Mock_CurrentConditionsProvider3 : CurrentConditionsProviderBase
        {
            public override QueryResult QueryCurrentConditions()
            {
                var queryResult = new QueryResult();
                queryResult.Error = "Network error";

                return queryResult;
            }
        }

        [TestMethod]
        public void ParseFailedCurrentConditionsData()
        {
            var cc = new CurrentConditions(new Mock_CurrentConditionsProvider3());

            Assert.IsNotNull(cc.Error);
            Assert.AreEqual("Network error", cc.Error);
        }

        class Mock_CurrentConditionsProvider4 : CurrentConditionsProviderBase
        {
            public override QueryResult QueryCurrentConditions()
            {
                var queryResult = new QueryResult();
                queryResult.XmlData = "<response>\n<vers";

                return queryResult;
            }
        }

        [TestMethod]
        public void ParseCorruptCurrentConditionsData()
        {
            var cc = new CurrentConditions(new Mock_CurrentConditionsProvider4());

            Assert.IsNotNull(cc.Error);
            Assert.AreEqual("Invalid XML", cc.Error);
        }

        class Mock_CurrentConditionsProvider5 : CurrentConditionsProviderBase
        {
            public override QueryResult QueryCurrentConditions()
            {
                throw new WebException();
            }
        }

        [TestMethod]
        public void HandleNetworkException()
        {
            var cc = new CurrentConditions(new Mock_CurrentConditionsProvider5());

            Assert.IsNotNull(cc.Error);
            Assert.AreEqual("Network error", cc.Error);
        }
    }
}