//=============================================================================|
// Copyright (C) 2015 Dan Watkins
// This file is licensed under the MIT License.
//=============================================================================|

using System.IO;
using System.Linq;

namespace Weather
{
    /**
     * Manages the caching of a single API key. 
     */
    public class APIKey
    {
        private string _value;

        /**
         * The actual API key. Assignments will cause the value to be written
         * to the cache file. Any IO exceptions will be thrown up.
         */
        public string Value
        {
            get { return _value; }

            set
            {
                _value = value;

                //dump value to cache file
                //if an IO exception occurs, that is the fault of the callee
                File.WriteAllText(_cacheFilePath, _value);
            }
        }

        private string _cacheFilePath = "";

        /**
         * Links the API key with the specified cache file so that an existing
         * key may be loaded. This also sets the cache file used for assignments
         * to the Value property.
         */
        public bool LinkCacheFile(string cacheFilePath)
        {
            _cacheFilePath = cacheFilePath;
            var fileLines = File.ReadAllLines(_cacheFilePath);

            if (fileLines.Count() != 1)
                return false;

            _value = fileLines[0];

            return true;
        }
    }
}