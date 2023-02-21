using pocketbase_csharp_sdk.Helper.Convert;
using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Models.Files;
using pocketbase_csharp_sdk.Sse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace pocketbase_csharp_sdk.Services
{
    public abstract class BaseSubCrudService : BaseService
    {
        private readonly PocketBase client;

        public BaseSubCrudService(PocketBase client)
        {
            this.client = client;
        }

        public async Task<PagedCollectionModel<T>> ListAsync<T>(string sub, int? page = null, int? perPage = null, string? sort = null, string? filter = null, string? expand = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default) where T : BaseModel
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
            var pagedResponse = await client.SendAsync<PagedCollectionModel<T>>(url, HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
            if (pagedResponse is null) throw new ClientException(url);

            return pagedResponse;
        }

        public async Task<IEnumerable<T>> GetFullListAsync<T>(string sub, int batch = 100, string? filter = null, string? sort = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            List<T> result = new();
            int currentPage = 1;
            PagedCollectionModel<T> lastResponse;
            do
            {
                lastResponse = await ListAsync<T>(sub, page: currentPage, perPage: batch, filter: filter, sort: sort, cancellationToken: cancellationToken);
                if (lastResponse is not null && lastResponse.Items is not null)
                {
                    result.AddRange(lastResponse.Items);
                }
                currentPage++;
            } while (lastResponse?.Items?.Length > 0 && lastResponse?.TotalItems > result.Count);

            return result;
        }

        public async Task<T> CreateAsync<T>(string sub, T item, string? expand = null, IDictionary<string, string>? headers = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(sub);
            var response = await client.SendAsync<T>(url, HttpMethod.Post, body: body, headers: headers, query: query, files: files, cancellationToken: cancellationToken);
            if (response is null) throw new ClientException(url);

            return response;
        }

        public async Task<T> UpdateAsync<T>(string sub, string id, T item, string? expand = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(sub) + "/" + UrlEncode(id);
            var response = await client.SendAsync<T>(url, HttpMethod.Patch, body: body, headers: headers, query: query, cancellationToken: cancellationToken);
            if (response is null) throw new ClientException(url);

            return response;
        }

        /// <summary>
        /// subscribe to the specified topic for realtime updates
        /// </summary>
        /// <param name="sub">the topic to subscribe to</param>
        /// <param name="recordId">the id of the specific record or * for the whole collection</param>
        /// <param name="callback">callback, that is invoked every time something changes</param>
        public async void Subscribe(string sub, string recordId, Func<SseMessage, Task> callback)
        {
            string subscribeTo = recordId != "*"
                    ? $"{sub}/{recordId}"
                    : sub;

            try
            {
                await client.RealTime.SubscribeAsync(subscribeTo, callback);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// unsubscribe all listeners from the specified topic
        /// </summary>
        /// <param name="topic">the topic to unsubscribe from</param>
        public Task UnsubscribeAsync(string? topic = null)
        {
            return client.RealTime.UnsubscribeAsync(topic);
        }

        /// <summary>
        /// unsubscribe all listeners from the specified prefix
        /// </summary>
        /// <param name="prefix">the prefix to unsubscribe from</param>
        /// <returns></returns>
        public Task UnsubscribeByPrefixAsync(string prefix)
        {
            return client.RealTime.UnsubscribeByPrefixAsync(prefix);
        }

        /// <summary>
        /// unsubscribe the specified listener from the specified topic
        /// </summary>
        /// <param name="topic">the topic to unsubscribe from</param>
        /// <param name="listener">the listener to remove</param>
        /// <returns></returns>
        public Task UnsubscribeByTopicAndListenerAsync(string topic, Func<SseMessage, Task> listener)
        {
            return client.RealTime.UnsubscribeByTopicAndListenerAsync(topic, listener);
        }


        public async Task UploadFileAsync(string sub, string field, string fileName, Stream stream, CancellationToken cancellationToken = default)
        {
            var file = new StreamFile()
            {
                FileName = fileName,
                FieldName = field,
                Stream = stream
            };
            var url = this.BasePath(sub);
            await client.SendAsync(url, HttpMethod.Post, files: new[] { file }, cancellationToken: cancellationToken);
        }

    }
}
