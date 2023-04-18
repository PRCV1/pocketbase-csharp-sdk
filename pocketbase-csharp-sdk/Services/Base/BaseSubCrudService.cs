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
using FluentResults;

namespace pocketbase_csharp_sdk.Services.Base
{
    public abstract class BaseSubCrudService : BaseService
    {
        private readonly PocketBase client;

        public BaseSubCrudService(PocketBase client) : base(client)
        {
            this.client = client;
        }

        public virtual Result<PagedCollectionModel<T>> List<T>(string sub, int page = 1, int perPage = 30, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            var path = BasePath(sub);
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };
        
            return client.Send<PagedCollectionModel<T>>(path, HttpMethod.Get, query: query, cancellationToken: cancellationToken);
        }

        public virtual Task<Result<PagedCollectionModel<T>>> ListAsync<T>(string sub, int page = 1, int perPage = 30, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            var path = BasePath(sub);
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };
            return client.SendAsync<PagedCollectionModel<T>>(path, HttpMethod.Get, query: query, cancellationToken: cancellationToken);;
        }

        public virtual Result<IEnumerable<T>> GetFullList<T>(string sub, int batch = 100, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            List<T> result = new();
            int currentPage = 1;
            Result<PagedCollectionModel<T>> lastResponse;
            do
            {
                lastResponse = List<T>(sub, page: currentPage, perPage: batch, filter: filter, sort: sort, cancellationToken: cancellationToken);
                if (lastResponse.IsSuccess && lastResponse.Value.Items is not null)
                {
                    result.AddRange(lastResponse.Value.Items);
                }
                currentPage++;
            } while (lastResponse.IsSuccess && lastResponse.Value.Items?.Count > 0 && lastResponse.Value.TotalItems > result.Count);

            return result;
        }

        public virtual async Task<Result<IEnumerable<T>>> GetFullListAsync<T>(string sub, int batch = 100, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            List<T> result = new();
            int currentPage = 1;
            Result<PagedCollectionModel<T>> lastResponse;
            do
            {
                lastResponse = await ListAsync<T>(sub, page: currentPage, perPage: batch, filter: filter, sort: sort, cancellationToken: cancellationToken);
                if (lastResponse.IsSuccess && lastResponse.Value.Items is not null)
                {
                    result.AddRange(lastResponse.Value.Items);
                }
                currentPage++;
            } while (lastResponse.IsSuccess && lastResponse.Value.Items?.Count > 0 && lastResponse.Value.TotalItems > result.Count);

            return result;
        }

        public virtual Result<T> GetOne<T>(string sub, string id)
        {
            string url = $"{BasePath(sub)}/{UrlEncode(id)}";
            return client.Send<T>(url, HttpMethod.Get);
        }
        
        public virtual Task<Result<T>> GetOneAsync<T>(string sub, string id)
        {
            string url = $"{BasePath(sub)}/{UrlEncode(id)}";
            return client.SendAsync<T>(url, HttpMethod.Get);
        }
        
        public Task<Result<T>> CreateAsync<T>(string sub, T item, string? expand = null, IDictionary<string, string>? headers = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(sub);
            return client.SendAsync<T>(url, HttpMethod.Post, body: body, headers: headers, query: query, files: files, cancellationToken: cancellationToken);
        }

        public Result<T> Create<T>(string sub, T item, string? expand = null, IDictionary<string, string>? headers = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(sub);
            return client.Send<T>(url, HttpMethod.Post, body: body, headers: headers, query: query, files: files, cancellationToken: cancellationToken);
        }

        public Task<Result<T>> UpdateAsync<T>(string sub, T item, string? expand = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(sub) + "/" + UrlEncode(item.Id);
            return client.SendAsync<T>(url, HttpMethod.Patch, body: body, headers: headers, query: query, cancellationToken: cancellationToken);
        }

        public Result<T> Update<T>(string sub, T item, string? expand = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(sub) + "/" + UrlEncode(item.Id);
            return client.Send<T>(url, HttpMethod.Patch, body: body, headers: headers, query: query, cancellationToken: cancellationToken);
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

        public void UploadFile(string sub, string field, string fileName, Stream stream, CancellationToken cancellationToken = default)
        {
            var file = new StreamFile()
            {
                FileName = fileName,
                FieldName = field,
                Stream = stream
            };
            var url = this.BasePath(sub);
            client.Send(url, HttpMethod.Post, files: new[] { file }, cancellationToken: cancellationToken);
        }

    }
}
