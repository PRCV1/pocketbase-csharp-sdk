using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Event
{
    public class RequestEventArgs : EventArgs
    {

        public Uri Url { get; }
        public HttpRequestMessage HttpRequest { get; }

        public RequestEventArgs(Uri url, HttpRequestMessage httpRequest)
        {
            Url = url;
            HttpRequest = httpRequest;
        }

    }
}
