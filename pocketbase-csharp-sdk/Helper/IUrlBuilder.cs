using System.Collections;

namespace pocketbase_csharp_sdk.Helper
{
    public interface IUrlBuilder
    {
        public Uri BuildUrl(string path, PbListQueryParams queryParameters);
        public IDictionary<string, IEnumerable> NormalizeQueryParameters(IDictionary<string, object?>? parameters);
    }
}