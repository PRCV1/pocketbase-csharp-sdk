using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace pocketbase_csharp_sdk.Services
{
    public abstract class BaseService
    {
        protected readonly string[] itemProperties;
        private readonly PocketBase pocketBase;

        public BaseService(PocketBase pocketBase)
        {
            this.itemProperties = this.GetPropertyNames().ToArray();
            this.pocketBase = pocketBase;
        }

        protected abstract string BasePath(string? path = null);

        protected Dictionary<string, object> ConstructBody(object item)
        {
            var body = new Dictionary<string, object>();

            foreach (var prop in item.GetType().GetProperties())
            {
                if (this.itemProperties.Contains(prop.Name)) continue;
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

        protected string UrlEncode(string param)
        {
            return HttpUtility.UrlEncode(param);
        }

        internal virtual PagedCollectionModel<T> List<T>(string? sub = null, int page = 1, int perPage = 30, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            var path = BasePath(sub);
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };

            var response = pocketBase.Send<PagedCollectionModel<T>>(path, HttpMethod.Get, query: query, cancellationToken: cancellationToken);
            if (response is null)
            {
                throw new ClientException(BasePath(path));
            }

            return response;
        }

        internal virtual async Task<PagedCollectionModel<T>> ListAsync<T>(string? sub = null, int page = 1, int perPage = 30, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            var path = BasePath(sub);
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };

            var response = await pocketBase.SendAsync<PagedCollectionModel<T>>(path, HttpMethod.Get, query: query, cancellationToken: cancellationToken);
            if (response is null)
            {
                throw new ClientException(path);
            }

            return response;
        }

        internal virtual IEnumerable<T> GetFullList<T>(string? sub = null, int batch = 100, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            List<T> result = new();
            int currentPage = 1;
            PagedCollectionModel<T> lastResponse;
            do
            {
                lastResponse = List<T>(sub, currentPage, perPage: batch, filter: filter, sort: sort, cancellationToken: cancellationToken);
                if (lastResponse is not null && lastResponse.Items is not null)
                {
                    result.AddRange(lastResponse.Items);
                }
                currentPage++;
            } while (lastResponse?.Items?.Length > 0 && lastResponse?.TotalItems > result.Count);

            return result;
        }

        internal virtual async Task<IEnumerable<T>> GetFullListAsync<T>(string? sub = null, int batch = 100, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            List<T> result = new();
            int currentPage = 1;
            PagedCollectionModel<T> lastResponse;
            do
            {
                lastResponse = await ListAsync<T>(sub, currentPage, perPage: batch, filter: filter, sort: sort, cancellationToken: cancellationToken);
                if (lastResponse is not null && lastResponse.Items is not null)
                {
                    result.AddRange(lastResponse.Items);
                }
                currentPage++;
            } while (lastResponse?.Items?.Length > 0 && lastResponse?.TotalItems > result.Count);

            return result;
        }

    }
}
