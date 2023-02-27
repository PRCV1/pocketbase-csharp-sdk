using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Models.Auth;
using pocketbase_csharp_sdk.Models.Log;
using System.Web;

namespace pocketbase_csharp_sdk.Services
{
    public class CollectionAuthService<T> : CollectionAuthService<RecordAuthModel<T>, T>
        where T : IBaseAuthModel
    {
        public CollectionAuthService(PocketBase client, string collectionName) : base(client, collectionName) { }
    }

    public class CollectionAuthService<R, T> : BaseAuthService<R>
        where R : RecordAuthModel<T>
        where T : IBaseAuthModel
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

        public R? AuthenticateViaOAuth2(string provider, string code, string codeVerifier, string redirectUrl, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("provider", provider);
            body.Add("code", code);
            body.Add("codeVerifier", codeVerifier);
            body.Add("redirectUrl", redirectUrl);

            var url = $"{BasePath()}/auth-via-oauth2";
            var result = client.Send<R>(url, HttpMethod.Post, headers: headers, body: body, query: query, cancellationToken: cancellationToken);

            SaveAuthentication(result);

            return result;
        }

        public Task RequestVerificationAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("email", email);

            var url = $"{BasePath()}/request-verification";
            return client.SendAsync(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        public void RequestVerification(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("email", email);

            var url = $"{BasePath()}/request-verification";
            client.Send(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
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

        public R? ConfirmVerification(string token, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", token);

            var url = $"{BasePath()}/confirm-verification";
            var result = client.Send<R>(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);

            SaveAuthentication(result);

            return result;
        }

        public Task RequestEmailChangeAsync(string newEmail, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("newEmail", newEmail);

            var url = $"{BasePath()}/request-email-change";
            return client.SendAsync(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        public void RequestEmailChange(string newEmail, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("newEmail", newEmail);

            var url = $"{BasePath()}/request-email-change";
            client.Send(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
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

        public R? ConfirmEmailChange(string emailChangeToken, string userPassword, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", emailChangeToken);
            body.Add("password", userPassword);

            var url = $"{BasePath()}/confirm-email-change";
            var result = client.Send<R>(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);

            SaveAuthentication(result);

            return result;
        }

        public Task<AuthMethodsList?> GetAuthenticationMethodsAsync(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/auth-methods";
            return client.SendAsync<AuthMethodsList>(url, HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
        }

        public AuthMethodsList? GetAuthenticationMethods(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/auth-methods";
            return client.Send<AuthMethodsList>(url, HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
        }

        public Task<IEnumerable<ExternalAuthModel>?> GetExternalAuthenticationMethodsAsync(string userId, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/records/{HttpUtility.HtmlEncode(userId)}/external-auths";
            return client.SendAsync<IEnumerable<ExternalAuthModel>>(url, HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
        }

        public IEnumerable<ExternalAuthModel>? GetExternalAuthenticationMethods(string userId, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/records/{HttpUtility.HtmlEncode(userId)}/external-auths";
            return client.Send<IEnumerable<ExternalAuthModel>>(url, HttpMethod.Get, headers: headers, query: query, cancellationToken: cancellationToken);
        }

        public Task UnlinkExternalAuthenticationAsync(string userId, string provider, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/records/{HttpUtility.HtmlEncode(userId)}/external-auths/{HttpUtility.HtmlEncode(provider)}";
            return client.SendAsync(url, HttpMethod.Delete, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        public void UnlinkExternalAuthentication(string userId, string provider, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/records/{HttpUtility.HtmlEncode(userId)}/external-auths/{HttpUtility.HtmlEncode(provider)}";
            client.Send(url, HttpMethod.Delete, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

    }
}
