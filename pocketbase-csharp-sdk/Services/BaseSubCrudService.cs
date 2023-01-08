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

        public async Task<PagedCollectionModel<T>> ListAsync<T>(
            string sub,
            int? page = null,
            int? perPage = null,
            string? sort = null,
            string? filter = null,
            string? expand = null,
            IDictionary<string, string>? headers = null, 
            CancellationToken cancellationToken = default) where T : BaseModel
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
            var pagedResponse = await client.SendAsync<PagedCollectionModel<T>>(
                url,
                HttpMethod.Get,
                headers: headers,
                query: query,
                cancellationToken: cancellationToken);
            if (pagedResponse is null) throw new ClientException(url);

            return pagedResponse;
        }

        public async Task<T> CreateAsync<T>(
            string sub,
            T item,
            string? expand = null,
            IDictionary<string, string>? headers = null,
            IEnumerable<IFile>? files = null,
            CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(sub);
            var response = await client.SendAsync<T>(
                url,
                HttpMethod.Post,
                body: body,
                headers: headers,
                query: query,
                files: files,
                cancellationToken: cancellationToken);
            if (response is null) throw new ClientException(url);

            return response;
        }

        public async Task<T> UpdateAsync<T>(
            string sub,
            string id,
            T item,
            string? expand = null,
            IDictionary<string, string>? headers = null, 
            CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(sub) + "/" + UrlEncode(id);
            var response = await client.SendAsync<T>(
                url,
                HttpMethod.Patch,
                body: body,
                headers: headers,
                query: query, 
                cancellationToken: cancellationToken);
            if (response is null) throw new ClientException(url);

            return response;
        }

        //TODO REALTIME
        public async void Subscribe(string sub, string recordId, Func<SseMessage, Task> callback)
        {
            string subscribeTo = recordId != "*"
                    ? $"{sub}/{recordId}"
                    : sub;

            try
            {
                await client.RealTime.SubscribeAsync(subscribeTo, callback);
                //await sse.ConnectAsync(callback);

                //Dictionary<string, object> body = new();
                //body.Add("clientId", sse.Id);
                //body.Add("subscriptions", new List<string> { subscribeTo , "tags" });

                //var url = $"api/realtime";
                //await client.SendAsync(url, HttpMethod.Post, body: body);

                //var url = client.BuildUrl("/api/realtime").ToString();
                //var responseStream = await client._httpClient.GetStreamAsync(url);

                //var buffer = new byte[4096];
                //while (true)
                //{
                //    var readCount = await responseStream.ReadAsync(buffer, 0, buffer.Length);
                //    if (readCount > 0)
                //    {
                //        var data = Encoding.UTF8.GetString(buffer, 0, readCount);

                //    }
                //    await Task.Delay(125);
                //}

                //using StreamReader reader = new StreamReader(response);
                //SseMessage message = new SseMessage();
                //while (!reader.EndOfStream)
                //{
                //    var line = await reader.ReadLineAsync();

                //    if (ProcessMessage(line, message))
                //    {
                //        callback(message);
                //        message = new SseMessage();
                //    }
                //}
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task UploadFileAsync(string sub, string field, string fileName, Stream stream, CancellationToken cancellationToken = default)
        {
            var file = new StreamFile()
            {
                FieldName = field,
                Stream = stream
            };
            var url = this.BasePath(sub);
            await client.SendAsync(url, HttpMethod.Post, files: new[] { file }, cancellationToken: cancellationToken);
        }

    }
}
