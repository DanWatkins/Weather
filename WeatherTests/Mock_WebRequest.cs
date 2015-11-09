using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeatherTests
{
    public class Mock_WebRequestCreate : IWebRequestCreate
    {
        static WebRequest _nextRequest;
        static object _lockObject = new object();

        static public WebRequest NextRequest
        {
            get { return _nextRequest; }
            set
            {
                lock (_lockObject)
                {
                    _nextRequest = value;
                }
            }
        }

        public WebRequest Create(Uri uri)
        {
            return _nextRequest;
        }

        public static Mock_WebRequest CreateMockWebRequest()
        {
            return new Mock_WebRequest();
        }
    }

    public class Mock_WebRequest : WebRequest
    {
        public override WebResponse GetResponse()
        {
            throw new WebException();
        }
    }
}