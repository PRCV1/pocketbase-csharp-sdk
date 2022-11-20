using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace pocketbase_csharp_sdk.Services
{
    public abstract class BaseSubCrudService : BaseService
    {
        private readonly PocketBase client;
        private readonly string[] itemProperties;

        public BaseSubCrudService(PocketBase client)
        {
            this.client = client;
            this.itemProperties = this.GetPropertyNames().ToArray();
        }

        public async Task<PagedCollectionModel<T>> ListAsync<T>(
            string sub,
            int? page = null,
            int? perPage = null,
            string? sort = null,
            string? filter = null,
            string? expand = null,
            IDictionary<string, string>? headers = null) where T : ItemBaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort },
                { "expand", expand },
            };
            var url = this.BasePath(sub);
            var pagedCollection = await client.SendAsync<PagedCollectionModel<T>>(
                url,
                HttpMethod.Get,
                headers: headers,
                query: query);
            if (pagedCollection is null) throw new ClientException(url);

            return pagedCollection;
        }

        public async Task<T> CreateAsync<T>(
            string sub,
            T item,
            string? expand = null,
            IDictionary<string, string>? headers = null) where T : ItemBaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(sub);
            var ret = await client.SendAsync<T>(
                url,
                HttpMethod.Post,
                body: body,
                headers: headers,
                query: query);
            if (ret is null) throw new ClientException(url);

            return ret;
        }

        public async Task<T> UpdateAsync<T>(
            string sub,
            string id,
            T item,
            string? expand = null,
            IDictionary<string, string>? headers = null) where T : ItemBaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(sub) + "/" + HttpUtility.UrlEncode(id);
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

        private Dictionary<string, object> ConstructBody<T>(T item)
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
