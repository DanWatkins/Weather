//=============================================================================|
// Copyright (C) 2015 Dan Watkins
// This file is licensed under the MIT License.
//=============================================================================|

namespace Weather
{
    //I'm sorry for using an abstract class like an interface. C# is apparently
    //against me declaring a nested class inside of an interface. Not sure why
    //that wouldn't work (trying to think of edge cases...)

    public abstract class CurrentConditionsProviderBase
    {
        public class QueryResult
        {
            public string Error { get; set; }

            public string XmlData { get; set; }
        }

        public abstract QueryResult QueryCurrentConditions();
    }


    public class CurrentConditionsProvider : CurrentConditionsProviderBase
    {
        public override QueryResult QueryCurrentConditions()
        {
            var queryResult = new QueryResult();
            queryResult.XmlData = "";

            return queryResult;
        }
    }
}
