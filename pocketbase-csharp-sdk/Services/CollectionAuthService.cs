using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Models.Auth;
using pocketbase_csharp_sdk.Models.Log;
using System.Web;

namespace pocketbase_csharp_sdk.Services
{
    public class CollectionAuthService<T> : CollectionAuthService<RecordAuthModel<T>, T>
        where T : BaseAuthModel
    {
        public CollectionAuthService(PocketBase client, string collectionName) : base(client, collectionName) { }
    }

    public class CollectionAuthService<R, T> : BaseAuthService<R>
        where R : RecordAuthModel<T>
        where T : BaseAuthModel
    {
        protected override string BasePath(string? url = null) => $"/api/collections/{this.collectionName}";

        private readonly string collectionName;


        public CollectionAuthService(PocketBase client, string collectionName) : base(client)
        {
            this.collectionName = collectionName;
        }

        public async Task<R?> AuthenticateViaOAuth2Async(string provider, string code, string codeVerifier, string redirectUrl, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("provider", provider);
            body.Add("code", code);
            body.Add("codeVerifier", codeVerifier);
            body.Add("redirectUrl", redirectUrl);

            var url = $"{BasePath()}/auth-via-oauth2";
            var result = await client.SendAsync<R>(url, HttpMethod.Post, headers: headers, body: body, query: query, cancellationToken: cancellationToken);

            SaveAuthentication(result);

            return result;
        }

        public async Task RequestVerificationAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("email", email);

            var url = $"{BasePath()}/request-verification";
            await client.SendAsync(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        public async Task<R?> ConfirmVerificationAsync(string token, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", token);

            var url = $"{BasePath()}/confirm-verification";
            var result = await client.SendAsync<R>(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);

            SaveAuthentication(result);

            return result;
        }

        public async Task RequestEmailChangeAsync(string newEmail, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("newEmail", newEmail);

            var url = $"{BasePath()}/request-email-change";
            await client.SendAsync(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        public async Task<R?> ConfirmEmailChangeAsync(string emailChangeToken, string userPassword, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", emailChangeToken);
            body.Add("password", userPassword);

            var url = $"{BasePath()}/confirm-email-change";
            var result = await client.SendAsync<R>(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);

            SaveAuthentication(result);

            return result;
        }

        public async Task<AuthMethodsList?> GetAuthenticationMethodsAsync(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/auth-methods";
            var response = await client.SendAsync<AuthMethodsList>(url, HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
            return response;
        }

        public async Task<IEnumerable<ExternalAuthModel>?> GetExternalAuthenticationMethodsAsync(string userId, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/records/{HttpUtility.HtmlEncode(userId)}/external-auths";
            var result = await client.SendAsync<IEnumerable<ExternalAuthModel>>(url, HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
            return result;
        }

        public async Task UnlinkExternalAuthenticationAsync(string userId, string provider, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/records/{HttpUtility.HtmlEncode(userId)}/external-auths/{HttpUtility.HtmlEncode(provider)}";
            await client.SendAsync(url, HttpMethod.Delete, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

    }
}
