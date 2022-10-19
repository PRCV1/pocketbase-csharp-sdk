using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk
{
    public class ClientException : Exception
    {

        /// <summary>
        /// The Url of the failed request
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Indicates whether the error is a result from request cancellation/abort
        /// </summary>
        public bool IsAbort { get; }

        /// <summary>
        /// The status code of the failed request
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Contains the JSON API error response
        /// </summary>
        public IDictionary<string, object?>? Response { get; }

        /// <summary>
        /// The original response error
        /// </summary>
        public Exception? OriginalError { get; }

        public ClientException(string url, bool isAbort = false, int statusCode = 500, IDictionary<string, object?>? response = null, Exception? originalError = null)
        {
            Url = url;
            IsAbort = isAbort;
            StatusCode = statusCode;
            Response = response;
            OriginalError = originalError;
        }

        public override string Message => FormatMessage();

        private string FormatMessage()
        {
            Dictionary<string, object?> result = new()
            {
                {"url", Url },
                {"isAbort", IsAbort },
                {"statusCode", StatusCode },
                {"response", Response },
                {"originalError", OriginalError },
            };

            return $"ClientException: {result}";
        }

        public override string ToString()
        {
            return $"ClientException: {FormatMessage()}";
        }

    }
}
