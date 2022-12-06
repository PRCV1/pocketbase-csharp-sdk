using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Models.Collection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace pocketbase_csharp_sdk.Services
{
    public abstract class BaseCrudService<T> : BaseService
    {
        private readonly PocketBase client;

        public BaseCrudService(PocketBase client)
        {
            this.client = client;
        }

        public virtual async Task<PagedCollectionModel<T>> ListAsync(int page = 1, int perPage = 30, string? filter = null, string? sort = null)
        {
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };

            var response = await client.SendAsync<PagedCollectionModel<T>>(BasePath(), HttpMethod.Get, query: query);
            if (response is null)
            {
                throw new ClientException(BasePath());
            }

            return response;
        }

        public virtual async Task<IEnumerable<T>> GetFullListAsync(int batch = 100, string? filter = null, string? sort = null)
        {
            List<T> result = new();
            int currentPage = 1;
            PagedCollectionModel<T> lastResponse;
            do
            {
                lastResponse = await ListAsync(currentPage, perPage: batch, filter: filter, sort: sort);
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
