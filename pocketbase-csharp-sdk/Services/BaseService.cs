using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public abstract class BaseService
    {
        protected readonly string[] itemProperties;

        public BaseService()
        {
            this.itemProperties = this.GetPropertyNames().ToArray();
        }

        protected abstract string BasePath(string? path = null);

        protected Dictionary<string, object> ConstructBody(object item)
        {
            var body = new Dictionary<string, object>();

            foreach (var prop in item.GetType().GetProperties())
            {
                if (this.itemProperties.Contains(prop.Name)) continue;
                var propValue = prop.GetValue(item, null);
                if (propValue is not null) body.Add(toCamelCase(prop.Name), propValue);
            }

            return body;
        }

        private string toCamelCase(string str)
        {
            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        private IEnumerable<string> GetPropertyNames()
            => from prop in typeof(BaseModel).GetProperties()
               select prop.Name;

    }
}
