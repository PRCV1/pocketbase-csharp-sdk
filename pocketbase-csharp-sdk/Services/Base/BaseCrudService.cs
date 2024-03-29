﻿using pocketbase_csharp_sdk.Models;
using FluentResults;

namespace pocketbase_csharp_sdk.Services.Base
{
    public abstract class BaseCrudService<T> : BaseService
    {
        private readonly PocketBase _client;

        protected BaseCrudService(PocketBase client)
        {
            this._client = client;
        }
        
        public virtual Result<PagedCollectionModel<T>> List(int page = 1, int perPage = 30, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            var path = BasePath();
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };
        
            return _client.Send<PagedCollectionModel<T>>(path, HttpMethod.Get, query: query, cancellationToken: cancellationToken);
        }

        public virtual Task<Result<PagedCollectionModel<T>>> ListAsync(int page = 1, int perPage = 30, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            var path = BasePath();
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };

            return _client.SendAsync<PagedCollectionModel<T>>(path, HttpMethod.Get, query: query, cancellationToken: cancellationToken);
        }

        public virtual Result<IEnumerable<T>> GetFullList(int batch = 100, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            List<T> result = new();
            int currentPage = 1;
            Result<PagedCollectionModel<T>> lastResponse;
            do
            {
                lastResponse = List(currentPage, perPage: batch, filter: filter, sort: sort, cancellationToken: cancellationToken);
                if (lastResponse.IsSuccess && lastResponse.Value.Items is not null)
                {
                    result.AddRange(lastResponse.Value.Items);
                }
                currentPage++;
            } while (lastResponse.IsSuccess && lastResponse.Value.Items?.Count > 0 && lastResponse.Value.TotalItems > result.Count);

            return result;
        }

        public virtual async Task<IEnumerable<T>> GetFullListAsync(int batch = 100, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            List<T> result = new();
            int currentPage = 1;
            Result<PagedCollectionModel<T>> lastResponse;
            do
            {
                lastResponse = await ListAsync(currentPage, perPage: batch, filter: filter, sort: sort, cancellationToken: cancellationToken);
                if (lastResponse.IsSuccess && lastResponse.Value.Items is not null)
                {
                    result.AddRange(lastResponse.Value.Items);
                }
                currentPage++;
            } while (lastResponse.IsSuccess && lastResponse.Value.Items?.Count > 0 && lastResponse.Value.TotalItems > result.Count);

            return result;
        }

        public virtual Result<T> GetOne(string id)
        {
            string url = $"{BasePath()}/{UrlEncode(id)}";
            return _client.Send<T>(url, HttpMethod.Get);
        }
        
        public virtual Task<Result<T>> GetOneAsync(string id)
        {
            string url = $"{BasePath()}/{UrlEncode(id)}";
            return _client.SendAsync<T>(url, HttpMethod.Get);
        }

    }
}
