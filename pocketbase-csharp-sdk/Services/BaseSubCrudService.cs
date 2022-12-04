using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var url = this.BasePath(sub) + "/" + HttpUtility.UrlEncode(id);
            var response = await client.SendAsync<T>(
                url,
                HttpMethod.Patch,
                body: body,
                headers: headers,
                query: query);
            if (response is null) throw new ClientException(url);

            return response;
        }

        public Task UploadFileAsync(string sub, string filePath)
        {
            if (File.Exists(filePath))
            {
                return Task.CompletedTask;
            }

            var fileName = Path.GetFileName(filePath);
            var readStream = File.OpenRead(filePath);

            return UploadFileAsync(sub, fileName, readStream);
        }

        public async Task UploadFileAsync(string sub, string fileName, Stream stream)
        {
            string fieldName = "file1";
            var file = new FileContentWrapper()
            {
                FileName = fileName,
                Stream = stream,
            };
            var body = new Dictionary<string, object>()
            {
                { fieldName, file }, 
            };
            await client.SendAsync(sub, HttpMethod.Post, body: body, files: new[] { file });
        }

    }
}
