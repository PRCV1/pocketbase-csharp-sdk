using pocketbase_csharp_sdk.Models;
using System.Web;

namespace pocketbase_csharp_sdk.Services.Base
{
    public abstract class BaseService
    {
        private readonly string[] _itemProperties;

        protected BaseService()
        {
            this._itemProperties = this.GetPropertyNames().ToArray();
        }

        protected abstract string BasePath(string? path = null);

        protected Dictionary<string, object> ConstructBody(object item)
        {
            var body = new Dictionary<string, object>();

            foreach (var prop in item.GetType().GetProperties())
            {
                if (_itemProperties.Contains(prop.Name)) continue;
                var propValue = prop.GetValue(item, null);
                if (propValue is not null) body.Add(ToCamelCase(prop.Name), propValue);
            }

            return body;
        }

        private string ToCamelCase(string str)
        {
            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        private IEnumerable<string> GetPropertyNames()
        {
            return from prop in typeof(BaseModel).GetProperties()
                   select prop.Name;
        }

        protected string UrlEncode(string? param)
        {
            return HttpUtility.UrlEncode(param) ?? "";
        }

    }
}
