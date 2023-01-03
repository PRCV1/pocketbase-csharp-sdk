using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Models.Auth;
using pocketbase_csharp_sdk.Models.Log;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace pocketbase_csharp_sdk.Services
{
    public class CollectionAuthService : BaseAuthService
    {
        protected override string BasePath(string? url = null) => $"/api/collections/{this.collectionName}";

        private readonly string collectionName;


        public CollectionAuthService(PocketBase client, string collectionName) : base(client) 
        { 
            this.collectionName = collectionName;
        }

        public async Task<AuthModel?> AuthenticateViaOAuth2(string provider, string code, string codeVerifier, string redirectUrl, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("provider", provider);
            body.Add("code", code);
            body.Add("codeVerifier", codeVerifier);
            body.Add("redirectUrl", redirectUrl);

            var url = $"{BasePath()}/auth-via-oauth2";
            var result = await client.SendAsync<AuthModel>(url, HttpMethod.Post, headers: headers, body: body, query: query);

            SaveAuthentication(result);

            return result;
        }

        public async Task RequestVerificationAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("email", email);

            var url = $"{BasePath()}/request-verification";
            await client.SendAsync(url, HttpMethod.Post, headers: headers, query: query, body: body);
        }

        public async Task<AuthModel?> ConfirmVerificationAsync(string token, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", token);

            var url = $"{BasePath()}/confirm-verification";
            var result = await client.SendAsync<AuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

            SaveAuthentication(result);

            return result;
        }

        public async Task RequestEmailChangeAsync(string newEmail, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("newEmail", newEmail);

            var url = $"{BasePath()}/request-email-change";
            await client.SendAsync(url, HttpMethod.Post, headers: headers, query: query, body: body);
        }

        public async Task<AuthModel?> ConfirmEmailChangeAsync(string emailChangeToken, string userPassword, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", emailChangeToken);
            body.Add("password", userPassword);

            var url = $"{BasePath()}/confirm-email-change";
            var result = await client.SendAsync<AuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

            SaveAuthentication(result);

            return result;
        }

        public async Task<AuthMethodsList?> GetAuthenticationMethodsAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath()}/auth-methods";
            var response = await client.SendAsync<AuthMethodsList>(url, HttpMethod.Get, headers: headers, query: query, body: body);
            return response;
        }

        public async Task<IEnumerable<ExternalAuthModel>?> GetExternalAuthenticationMethods(string userId, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath()}/records/{HttpUtility.HtmlEncode(userId)}/external-auths";
            var result = await client.SendAsync<IEnumerable<ExternalAuthModel>>(url, HttpMethod.Get, headers: headers, query: query);
            return result;
        }

        public async Task UnlinkExternalAuthentication(string userId, string provider, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath()}/records/{HttpUtility.HtmlEncode(userId)}/external-auths/{HttpUtility.HtmlEncode(provider)}";
            await client.SendAsync(url, HttpMethod.Delete, headers: headers, query: query, body: body);
        }

    }
}
