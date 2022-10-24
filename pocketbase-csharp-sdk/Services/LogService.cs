﻿using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace pocketbase_csharp_sdk.Services
{
    public class LogService
    {
        private readonly PocketBase client;

        public LogService(PocketBase client)
        {
            this.client = client;
        }

        public async Task<LogRequestModel?> GetRequestAsync(string id, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"api/logs/requests/{HttpUtility.HtmlEncode(id)}";
            var response = await client.SendAsync<LogRequestModel>(url, HttpMethod.Get, headers: headers, query: query, body: body);
            return response;
        }

        public async Task<ResultList<LogRequestModel>?> GetRequestsAsync(int page = 1, int perPage = 30, string? filter = null, string? sort = null, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            query ??= new Dictionary<string, object?>();
            query.Add("page", page);
            query.Add("perPage", perPage);
            query.Add("filter", filter);
            query.Add("sort", sort);

            var url = "api/logs/requests/";
            var response = await client.SendAsync<ResultList<LogRequestModel>>(url, HttpMethod.Get, headers: headers, query: query, body: body);

            return response;
        }

        public async Task<IEnumerable<LogRequestStatistic>?> GetRequestsStatisticsAsync(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = "/api/logs/requests/stats";
            var response = await client.SendAsync<IEnumerable<LogRequestStatistic>>(url, HttpMethod.Get, headers: headers, query: query);

            return response;
        }

    }
}