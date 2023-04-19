using FluentResults;
using pocketbase_csharp_sdk.Services.Base;

namespace pocketbase_csharp_sdk.Services
{
    public class SettingsService : BaseService
    {

        protected override string BasePath(string? path = null) => "api/settings";

        private readonly PocketBase _client;

        public SettingsService(PocketBase client)
        {
            this._client = client;
        }

        public Task<Result<IDictionary<string, object>>> GetAllAsync(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return _client.SendAsync<IDictionary<string, object>>(BasePath(), HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
        }

        public Result<IDictionary<string, object>> GetAll(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return _client.Send<IDictionary<string, object>>(BasePath(), HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
        }

        public Task<Result<IDictionary<string, object>>> UpdateAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return _client.SendAsync<IDictionary<string, object>>(BasePath(), HttpMethod.Patch, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        public Result<IDictionary<string, object>> Update(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return _client.Send<IDictionary<string, object>>(BasePath(), HttpMethod.Patch, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        public Task<Result> TestS3Async(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/test/s3";
            return _client.SendAsync(url, HttpMethod.Post, headers: headers, body: body, query: query, cancellationToken: cancellationToken);
        }

        public Result TestS3(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/test/s3";
            return _client.Send(url, HttpMethod.Post, headers: headers, body: body, query: query, cancellationToken: cancellationToken);
        }

        public Task<Result> TestEmailAsync(string toEmail, string template, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            query ??= new Dictionary<string, object?>();
            query.Add("email", toEmail);
            query.Add("template", template);

            var url = $"{BasePath()}/test/email";
            return _client.SendAsync(url, HttpMethod.Post, headers: headers, body: body, query: query, cancellationToken: cancellationToken);
        }

        public Result TestEmail(string toEmail, string template, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            query ??= new Dictionary<string, object?>();
            query.Add("email", toEmail);
            query.Add("template", template);

            var url = $"{BasePath()}/test/email";
            return _client.Send(url, HttpMethod.Post, headers: headers, body: body, query: query, cancellationToken: cancellationToken);
        }

    }
}
