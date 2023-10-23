using System.Collections;
using System.Collections.Specialized;
using System.Web;

namespace pocketbase_csharp_sdk.Helper
{
    public class UrlBuilder : IUrlBuilder
    {
        private readonly string _baseUrl;

        public UrlBuilder(string baseUrl)
        {
            _baseUrl = baseUrl;
        }
        
        public Uri BuildUrl(string path, PbListQueryParams queryParameters)
        {
            var url = _baseUrl + (_baseUrl.EndsWith("/") ? "" : "/");

            if (!string.IsNullOrWhiteSpace(path))
            {
                url += path.StartsWith("/") ? path.Substring(1) : path;
            }

            if (!url.EndsWith("/"))
            {
                url += "/";
            }
            
            var emptyQuery = HttpUtility.ParseQueryString("");
            var queryDictionary = queryParameters.ToDictionary();
            foreach (var kvp in queryDictionary)
            {
                emptyQuery.Add(kvp.Key, kvp.Value);
            }

            var fullUrl = url + emptyQuery;
            return new Uri(fullUrl, UriKind.RelativeOrAbsolute);
            
            
            // if (queryParameters is not null)
            // {
            //     var query = NormalizeQueryParameters(queryParameters);
            //
            //     List<string> urlSegments = new();
            //     foreach (var kvp in query)
            //     {
            //         var encodedKey = HttpUtility.UrlEncode(kvp.Key);
            //         foreach (var item in kvp.Value)
            //         {
            //             var encodedValue = HttpUtility.UrlEncode(item.ToString());
            //             urlSegments.Add($"{encodedKey}={encodedValue}");
            //         }
            //     }
            //
            //     var queryString = string.Join("&", urlSegments);
            //
            //     if (!string.IsNullOrWhiteSpace(queryString))
            //     {
            //         url = url + "?" + queryString;
            //     }
            // }
            //
            // return new Uri(url, UriKind.RelativeOrAbsolute);
        }
        
        public IDictionary<string, IEnumerable> NormalizeQueryParameters(IDictionary<string, object?>? parameters)
        {
            Dictionary<string, IEnumerable> result = new();

            if (parameters is null)
            {
                return result;
            }

            foreach (var item in parameters)
            {
                List<string> normalizedValue = new();
                IEnumerable valueAsList;

                if (item.Value is IEnumerable && item.Value is not string)
                {
                    valueAsList = (IEnumerable)item.Value;
                }
                else
                {
                    valueAsList = new List<object?>() { item.Value };
                }

                foreach (var subItem in valueAsList)
                {
                    if (subItem is null)
                    {
                        continue;
                    }
                    normalizedValue.Add(subItem.ToString() ?? "");
                }

                if (normalizedValue.Count > 0)
                {
                    result[item.Key] = normalizedValue;
                }
            }

            return result;
        }
    }
}