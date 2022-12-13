using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Event
{
    public class ResponseEventArgs : EventArgs
    {

        public Uri Url { get; }
        public HttpResponseMessage? HttpResponse { get; }

        public ResponseEventArgs(Uri url, HttpResponseMessage? httpResponse)
        {
            Url = url;
            HttpResponse = httpResponse;
        }

    }
}
