//=============================================================================|
// Copyright (C) 2015 Dan Watkins
// This file is licensed under the MIT License.
//=============================================================================|

namespace Weather
{
    public interface ICurrentConditionsProvider
    {
        string QueryCurrentConditions();
    }


    public class CurrentConditionsProvider : ICurrentConditionsProvider
    {
        string ICurrentConditionsProvider.QueryCurrentConditions()
        {
            return "";
        }
    }
}
