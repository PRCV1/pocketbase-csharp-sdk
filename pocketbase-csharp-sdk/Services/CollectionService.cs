using pocketbase_csharp_sdk.Models.Collection;
using FluentResults;
using pocketbase_csharp_sdk.Services.Base;

namespace pocketbase_csharp_sdk.Services
{
    public class CollectionService : BaseCrudService<CollectionModel>
    {
        private readonly PocketBase _client;

        protected override string BasePath(string? url = null) => "/api/collections";

        public CollectionService(PocketBase client) : base(client)
        {
            this._client = client;
        }

        public Task<Result> ImportAsync(IEnumerable<CollectionModel> collections, bool deleteMissing = false, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("collections", collections);
            body.Add("deleteMissing", deleteMissing);

            var url = $"{BasePath()}/import";
            return _client.SendAsync(url, HttpMethod.Put, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        public Result Import(IEnumerable<CollectionModel> collections, bool deleteMissing = false, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("collections", collections);
            body.Add("deleteMissing", deleteMissing);

            var url = $"{BasePath()}/import";
            return _client.Send(url, HttpMethod.Put, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        public Task<Result<CollectionModel>> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/{UrlEncode(name)}";
            return _client.SendAsync<CollectionModel>(url, HttpMethod.Get, cancellationToken: cancellationToken);
        }

        public Result<CollectionModel> GetByName(string name, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/{UrlEncode(name)}";
            return _client.Send<CollectionModel>(url, HttpMethod.Get, cancellationToken: cancellationToken);
        }

        public Task<Result> DeleteAsync(string name, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/{UrlEncode(name)}";
            return _client.SendAsync(url, HttpMethod.Delete, cancellationToken: cancellationToken);
        }

        public Result Delete(string name, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/{UrlEncode(name)}";
            return _client.Send(url, HttpMethod.Delete, cancellationToken: cancellationToken);
        }

    }
}
