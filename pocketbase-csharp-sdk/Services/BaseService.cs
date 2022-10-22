using pocketbase_csharp_sdk.Models;
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
        private readonly string[] itemProperties;

        public BaseService(PocketBase client)
        {
            this.client = client;
            this.itemProperties = this.GetPropertyNames().ToArray();
        }

        public async Task<PagedCollectionModel<T>> ListAsync(
            int? page = null,
            int? perPage = null,
            string? sort = null,
            string? filter = null,
            string? expand = null,
            IDictionary<string, string>? headers = null)
        {
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort },
                { "expand", expand },
            };
            var url = this.BasePath + "/records";
            var pagedCollection = await client.SendAsync<PagedCollectionModel<T>>(
                url,
                HttpMethod.Get,
                headers: headers,
                query: query);
            if (pagedCollection is null) throw new ClientException(url);

            return pagedCollection;
        }

        public async Task<T> CreateAsync(
            T item,
            string? expand = null,
            IDictionary<string, string>? headers = null)
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath + "/records";
            var ret = await client.SendAsync<T>(
                url,
                HttpMethod.Post,
                body: body,
                headers: headers,
                query: query);
            if (ret is null) throw new ClientException(url);

            return ret;
        }

        public async Task<T> UpdateAsync(
            string id,
            T item,
            string? expand = null,
            IDictionary<string, string>? headers = null)
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath + "/records/" + id;
            var pagedCollection = await client.SendAsync<T>(
                url,
                HttpMethod.Patch,
                body: body,
                headers: headers,
                query: query);
            if (pagedCollection is null) throw new ClientException(url);

            return pagedCollection;
        }

        private IEnumerable<string> GetPropertyNames()
            => from prop in typeof(ItemBaseModel).GetProperties()
               select prop.Name;

        private Dictionary<string, object> ConstructBody(T item)
        {
            var body = new Dictionary<string, object>();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (this.itemProperties.Contains(prop.Name)) continue;
                var propValue = prop.GetValue(item, null);
                if (propValue is not null) body.Add(toCamelCase(prop.Name), propValue);
            }

            string toCamelCase(string str)
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }

            return body;
        }
    }
}
