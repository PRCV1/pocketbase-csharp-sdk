using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public class SettingsService : BaseService
    {

        protected override string BasePath(string? path = null) => "api/settings";

        private readonly PocketBase client;

        public SettingsService(PocketBase client) : base(client)
        {
            this.client = client;
        }

        public Task<IDictionary<string, object>?> GetAllAsync(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return client.SendAsync<IDictionary<string, object>>(BasePath(), HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
        }

        public IDictionary<string, object>? GetAll(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return client.Send<IDictionary<string, object>>(BasePath(), HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
        }

        public Task<IDictionary<string, object>?> UpdateAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return client.SendAsync<IDictionary<string, object>>(BasePath(), HttpMethod.Patch, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        public IDictionary<string, object>? Update(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return client.Send<IDictionary<string, object>>(BasePath(), HttpMethod.Patch, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        public Task TestS3Async(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/test/s3";
            return client.SendAsync(url, HttpMethod.Post, headers: headers, body: body, query: query, cancellationToken: cancellationToken);
        }

        public void TestS3(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/test/s3";
            client.Send(url, HttpMethod.Post, headers: headers, body: body, query: query, cancellationToken: cancellationToken);
        }

        public Task TestEmailAsync(string toEmail, string template, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            query ??= new Dictionary<string, object?>();
            query.Add("email", toEmail);
            query.Add("template", template);

            var url = $"{BasePath()}/test/email";
            return client.SendAsync(url, HttpMethod.Post, headers: headers, body: body, query: query, cancellationToken: cancellationToken);
        }

        public void TestEmail(string toEmail, string template, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            query ??= new Dictionary<string, object?>();
            query.Add("email", toEmail);
            query.Add("template", template);

            var url = $"{BasePath()}/test/email";
            client.Send(url, HttpMethod.Post, headers: headers, body: body, query: query, cancellationToken: cancellationToken);
        }

    }
}
