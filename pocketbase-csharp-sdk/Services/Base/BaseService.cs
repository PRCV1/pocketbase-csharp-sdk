using System.Web;

namespace pocketbase_csharp_sdk.Services.Base
{
    public abstract class BaseService
    {
        protected string UrlEncode(string? param)
        {
            return HttpUtility.UrlEncode(param) ?? "";
        }
    }
}