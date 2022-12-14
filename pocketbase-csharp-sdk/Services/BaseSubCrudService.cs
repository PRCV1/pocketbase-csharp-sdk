using pocketbase_csharp_sdk.Helper.Convert;
using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            IDictionary<string, string>? headers = null) where T : BaseModel
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
                query: query);
            if (pagedResponse is null) throw new ClientException(url);

            return pagedResponse;
        }

        public async Task<T> CreateAsync<T>(
            string sub,
            T item,
            string? expand = null,
            IDictionary<string, string>? headers = null,
            IEnumerable<FileContentWrapper>? files = null) where T : BaseModel
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
                query: query);
            if (response is null) throw new ClientException(url);

            return response;
        }

        public async Task<T> UpdateAsync<T>(
            string sub,
            string id,
            T item,
            string? expand = null,
            IDictionary<string, string>? headers = null) where T : BaseModel
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
                query: query);
            if (response is null) throw new ClientException(url);

            return response;
        }

        //TODO REALTIME
        public void Subscribe(string sub, string recordId, Action<SseMessage> callback)
        {
            Task.Run(async () =>
            {
                string subscribeTo = recordId != "*"
                    ? $"{sub}/{recordId}"
                    : sub;
                
                try
                {
                    var url = client.BuildUrl("/api/realtime").ToString();
                    var response = await client._httpClient.GetStreamAsync(url);

                    using StreamReader reader = new StreamReader(response);
                    SseMessage message = new SseMessage();
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        
                        if (ProcessMessage(line, message))
                        {
                            callback(message);
                            message = new SseMessage();
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            });
        }

        private bool ProcessMessage(string? line, SseMessage message)
        {
            Regex regex = new Regex("^(\\w+)[\\s\\:]+(.*)?$");
            if (string.IsNullOrWhiteSpace(line))
            {
                return true;
            }

            var match = regex.Match(line);
            if (match is null)
            {
                return false;
            }

            var field = match.Groups[1].Value ?? "";
            var value = match.Groups[2].Value ?? "";

            switch (field)
            {
                case "id":
                    message.Id = value;
                    break;
                case "event":
                    message.Event = value;
                    break;
                case "retry":
                    message.Retry = SafeConvert.ToInt(value, 0);
                    break;
                case "data":
                    message.Data = value;
                    break;
            }

            return false;
        }

        public Task UploadFileAsync(string sub, string filePath)
        {
            if (!File.Exists(filePath))
            {
                return Task.CompletedTask;
            }

            var fileName = Path.GetFileName(filePath);
            var readStream = File.OpenRead(filePath);

            return UploadFileAsync(sub, fileName, readStream);
        }

        public async Task UploadFileAsync(string sub, string fileName, Stream stream)
        {
            var file = new FileContentWrapper()
            {
                FileName = fileName,
                Stream = stream,
            };
            await client.SendAsync(sub, HttpMethod.Post, files: new[] { file });
        }

    }
}
