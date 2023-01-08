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

        /// <summary>
        /// retrieves a paginated list of objects of type T
        /// </summary>
        /// <param name="page">The page number of the list to retrieve. Default is 1</param>
        /// <param name="perPage">The number of objects per page. Default is 30.</param>
        /// <param name="filter">A filter string to apply to the list. Default is null.</param>
        /// <param name="sort">A sort string to apply to the list. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token</param>
        /// <returns>A PagedCollectionModel<T> object containing the paginated list of objects.</returns>
        /// <exception cref="ClientException"></exception>
        public virtual async Task<PagedCollectionModel<T>> ListAsync(int page = 1, int perPage = 30, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };

            var response = await client.SendAsync<PagedCollectionModel<T>>(BasePath(), HttpMethod.Get, query: query, cancellationToken: cancellationToken);
            if (response is null)
            {
                throw new ClientException(BasePath());
            }

            return response;
        }

        /// <summary>
        /// retrieves a full list of objects of type T, by making multiple calls to the `ListAsync` function and paginating through the results.
        /// </summary>
        /// <param name="batch">The number of objects to retrieve per call to `ListAsync`. Default is 100.</param>
        /// <param name="filter">A filter string to apply to the list. Default is null.</param>
        /// <param name="sort">A sort string to apply to the list. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>An IEnumerable<T> object containing the full list of objects.</returns>
        public virtual async Task<IEnumerable<T>> GetFullListAsync(int batch = 100, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            List<T> result = new();
            int currentPage = 1;
            PagedCollectionModel<T> lastResponse;
            do
            {
                lastResponse = await ListAsync(currentPage, perPage: batch, filter: filter, sort: sort, cancellationToken: cancellationToken);
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
