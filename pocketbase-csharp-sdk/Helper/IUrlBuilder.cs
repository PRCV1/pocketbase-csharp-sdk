using System.Collections;

namespace pocketbase_csharp_sdk.Helper
{
    public interface IUrlBuilder
    {
        public Uri BuildUrl(string path, IPbQueryParams? queryParameters = null);
        public IDictionary<string, IEnumerable> NormalizeQueryParameters(IDictionary<string, object?>? parameters);
    }
}