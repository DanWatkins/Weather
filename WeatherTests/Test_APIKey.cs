//=============================================================================|
// Copyright (C) 2015 Dan Watkins
// This file is licensed under the MIT License.
//=============================================================================|

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Weather;
using System.IO;
using System;

namespace WeatherTests
{
    [TestClass]
    public class Test_APIKey
    {
        [TestMethod]
        public void NoKeyFound()
        {
            APIKey apiKey = new APIKey();

            Assert.IsNull(apiKey.Value);
        }

        [TestMethod]
        public void CacheKeySet()
        {
            const string someApiKeyString = "b13a56ac651acb6d550";
            string tempFile = Path.GetTempFileName();
            
            //create APIKey instance with a blank file
            var apiKey1 = new APIKey();
            Assert.IsFalse(apiKey1.LinkCacheFile(tempFile));
            Assert.IsNull(apiKey1.Value);

            //set the APIKey value to a real string
            apiKey1.Value = someApiKeyString;
            Assert.AreEqual(someApiKeyString, apiKey1.Value);

            //create another APIKey using the same file
            var apiKey2 = new APIKey();
            Assert.IsTrue(apiKey2.LinkCacheFile(tempFile));
            Assert.AreEqual(someApiKeyString, apiKey2.Value);
        }


        [TestMethod]
        public void FileDoesNotExistDuringLinkCachFile()
        {
            var apiKey = new APIKey();

            try
            {
                apiKey.LinkCacheFile("");
            }
            catch (ArgumentException)
            {
                return;
            }

            Assert.Fail("ArgumentException was not thrown when linking to a non-existent file.");
        }

        [TestMethod]
        public void FileDoesNotExistDuringValueAssignment()
        {
            const string someApiKeyString = "b13a56ac651acb6d550";
            string tempFile = Path.GetTempFileName();

            var apiKey = new APIKey();
            apiKey.LinkCacheFile(tempFile);

            File.Delete(tempFile);

            try
            {
                apiKey.Value = someApiKeyString;
            }
            catch (ArgumentException)
            {
                return;
            }

            Assert.Fail("ArgumentException was not thrown when attempting to cache the value to a non-existent file.");
        }
    }
}
