using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public abstract class BaseService<T> where T : class
    {
        protected abstract string BasePath { get; }

        private readonly PocketBase client;

        public BaseService(PocketBase client)
        {
            this.client = client;
        }

        protected async Task<IEnumerable<T>> GetFullListAsync(int batch = 100, string? filter = null, string? sort = null, IDictionary<string, object>? query = null, IDictionary<string, string>? headers = null)
        {
            var response = await client.SendAsync<IEnumerable<T>>(BasePath, HttpMethod.Get, headers: headers, query: query);
            return default;
        }

    }
}
