using System.Collections;

namespace pocketbase_csharp_sdk.Helper
{
    public interface IPbQueryParams
    {
        public IDictionary<string, string> ToDictionary();
    }
}