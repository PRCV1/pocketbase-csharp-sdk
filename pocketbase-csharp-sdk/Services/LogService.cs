﻿using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Models.Log;
using FluentResults;
using pocketbase_csharp_sdk.Services.Base;

namespace pocketbase_csharp_sdk.Services
{
    public class LogService : BaseService
    {

        protected override string BasePath(string? path = null) => "api/logs/requests";

        private readonly PocketBase _client;

        public LogService(PocketBase client)
        {
            this._client = client;
        }

        /// <summary>
        /// retrieves a specific request log from the API by its ID.
        /// </summary>
        /// <param name="id">The ID of the request log to retrieve.</param>
        /// <param name="body">The request body to send to the API. Default is null.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>A LogRequestModel object containing the request log.</returns>
        public Task<Result<LogRequestModel>> GetRequestAsync(string id, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/{UrlEncode(id)}";
            return _client.SendAsync<LogRequestModel>(url, HttpMethod.Get, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// retrieves a specific request log from the API by its ID.
        /// </summary>
        /// <param name="id">The ID of the request log to retrieve.</param>
        /// <param name="body">The request body to send to the API. Default is null.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>A LogRequestModel object containing the request log.</returns>
        public Result<LogRequestModel> GetRequest(string id, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/{UrlEncode(id)}";
            return _client.Send<LogRequestModel>(url, HttpMethod.Get, headers: headers, query: query, body: body, cancellationToken: cancellationToken); ;
        }

        /// <summary>
        /// retrieves a paginated list of request logs from the API.
        /// </summary>
        /// <param name="page">The page number of the list to retrieve. Default is 1.</param>
        /// <param name="perPage">The number of request logs per page. Default is 30.</param>
        /// <param name="filter">A filter string to apply to the list. Default is null.</param>
        /// <param name="sort">A sort string to apply to the list. Default is null.</param>
        /// <param name="body">The request body to send to the API. Default is null.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>A ResultList<LogRequestModel> object containing the paginated list of request logs.</returns>
        public Task<Result<ResultList<LogRequestModel>>> GetRequestsAsync(int page = 1, int perPage = 30, string? filter = null, string? sort = null, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            query ??= new Dictionary<string, object?>();
            query.Add("page", page);
            query.Add("perPage", perPage);
            query.Add("filter", filter);
            query.Add("sort", sort);

            return _client.SendAsync<ResultList<LogRequestModel>>(BasePath(), HttpMethod.Get, headers: headers, query: query, body: body, cancellationToken: cancellationToken); ;
        }

        /// <summary>
        /// retrieves a paginated list of request logs from the API.
        /// </summary>
        /// <param name="page">The page number of the list to retrieve. Default is 1.</param>
        /// <param name="perPage">The number of request logs per page. Default is 30.</param>
        /// <param name="filter">A filter string to apply to the list. Default is null.</param>
        /// <param name="sort">A sort string to apply to the list. Default is null.</param>
        /// <param name="body">The request body to send to the API. Default is null.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>A ResultList<LogRequestModel> object containing the paginated list of request logs.</returns>
        public Result<ResultList<LogRequestModel>> GetRequests(int page = 1, int perPage = 30, string? filter = null, string? sort = null, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            query ??= new Dictionary<string, object?>();
            query.Add("page", page);
            query.Add("perPage", perPage);
            query.Add("filter", filter);
            query.Add("sort", sort);

            return _client.Send<ResultList<LogRequestModel>>(BasePath(), HttpMethod.Get, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// retrieves statistics for the request logs
        /// </summary>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>An IEnumerable<LogRequestStatistic> object containing the request log statistics.</returns>
        public Task<Result<IEnumerable<LogRequestStatistic>>> GetRequestsStatisticsAsync(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/stats";
            return _client.SendAsync<IEnumerable<LogRequestStatistic>>(url, HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// retrieves statistics for the request logs
        /// </summary>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>An IEnumerable<LogRequestStatistic> object containing the request log statistics.</returns>
        public Result<IEnumerable<LogRequestStatistic>> GetRequestsStatistics(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/stats";
            return _client.Send<IEnumerable<LogRequestStatistic>>(url, HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
        }

    }
}
