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

        public SettingsService(PocketBase client)
        {
            this.client = client;
        }

        public async Task<IDictionary<string, object>?> GetAllAsync(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var response = await client.SendAsync<IDictionary<string, object>>(BasePath(), HttpMethod.Get, headers: headers, query: query);
            return response;
        }

        public async Task<IDictionary<string, object>?> UpdateAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var response = await client.SendAsync<IDictionary<string, object>>(BasePath(), HttpMethod.Patch, headers: headers, query: query, body: body);
            return response;
        }

        public async Task TestS3Async(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath()}/test/s3";
            await client.SendAsync(url, HttpMethod.Post, headers: headers, body: body, query: query);
        }

        public async Task TestEmailAsync(string toEmail, string template, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            query ??= new Dictionary<string, object?>();
            query.Add("email", toEmail);
            query.Add("template", template);

            var url = $"{BasePath()}/test/email";
            await client.SendAsync(url, HttpMethod.Post, headers: headers, body: body, query: query);
        }

    }
}
