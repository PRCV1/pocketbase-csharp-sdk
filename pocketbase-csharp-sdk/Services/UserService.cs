using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Models.Auth;
using pocketbase_csharp_sdk.Models.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace pocketbase_csharp_sdk.Services
{
    public class UserService : BaseCrudService<UserModel>
    {
        protected override string BasePath(string? url = null) => "/api/collections/users";

        private readonly PocketBase client;

        public UserService(PocketBase client) : base(client)
        {
            this.client = client;
        }

        public async Task<UserModel> CreateAsync(string email, string password, string passwordConfirm)
        {
            Dictionary<string, object> body = new()
            {
                { "email", email },
                { "password", password },
                { "passwordConfirm", passwordConfirm },
            };

            var response = await client.SendAsync<UserModel>(BasePath(), HttpMethod.Post, body: body);
            if (response is null)
            {
                throw new ClientException(BasePath());
            }

            return response;
        }

        public async Task<IEnumerable<UserModel>> GetFullListAsync(int batch = 100, string? filter = null, string? sort = null)
        {
            List<UserModel> result = new();

            int currentPage = 1;
            PagedCollectionModel<UserModel> lastResponse;
            do
            {
                lastResponse = await ListAsync(currentPage, perPage: batch, filter: filter, sort: sort);
                if (lastResponse is not null && lastResponse.Items is not null)
                {
                    result.AddRange(lastResponse.Items);
                }
                currentPage++;
            } while (lastResponse?.Items?.Length > 0 && lastResponse?.TotalItems > result.Count);

            return result;

            //var query = new Dictionary<string, object?>()
            //{
            //    { "filter", filter },
            //    { "page", page },
            //    { "perPage", perPage },
            //    { "sort", sort }
            //};
        }

        public async Task<PagedCollectionModel<UserModel>> ListAsync(int page = 1, int perPage = 30, string? filter = null, string? sort = null)
        {
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };

            var response = await client.SendAsync<PagedCollectionModel<UserModel>>(BasePath(), HttpMethod.Get, query: query);
            if (response is null)
            {
                throw new ClientException(BasePath());
            }

            return response;
        }

        public async Task<UserModel> GetOne(string id)
        {
            string url = $"{BasePath()}/{HttpUtility.UrlEncode(id)}";
            var result = await client.SendAsync<UserModel>(url, HttpMethod.Get);
            return result;
        }

        public async Task<UserModel> UpdateAsync()
        {
            //TODO
            return default;
        }

        public async Task<AuthMethodsList?> GetAuthenticationMethodsAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath()}/auth-methods";
            var response = await client.SendAsync<AuthMethodsList>(url, HttpMethod.Get, headers: headers, query: query, body: body);
            return response;
        }

        public async Task<UserAuthModel?> AuthenticateWithPassword(string email, string password, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("identity", email);
            body.Add("password", password);

            var url = $"{BasePath()}/auth-with-password";
            var result = await client.SendAsync<UserAuthModel>(url, HttpMethod.Post, headers: headers, body: body, query: query);

            SaveAuthentication(result);

            return result;
        }

        public async Task<UserAuthModel?> AuthenticateViaOAuth2(string provider, string code, string codeVerifier, string redirectUrl, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("provider", provider);
            body.Add("code", code);
            body.Add("codeVerifier", codeVerifier);
            body.Add("redirectUrl", redirectUrl);

            var url = $"{BasePath()}/auth-via-oauth2";
            var result = await client.SendAsync<UserAuthModel>(url, HttpMethod.Post, headers: headers, body: body, query: query);

            SaveAuthentication(result);

            return result;
        }

        public async Task<UserAuthModel?> RefreshAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath()}/refresh";
            var result = await client.SendAsync<UserAuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

            SaveAuthentication(result);

            return result;
        }

        public async Task RequestPasswordResetAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("email", email);

            var url = $"{BasePath()}/request-password-reset";
            await client.SendAsync(url, HttpMethod.Post, headers: headers, query: query, body: body);
        }

        public async Task<UserAuthModel?> ConfirmPasswordResetAsync(string passwordResetToken, string password, string passwordConfirm, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", passwordResetToken);
            body.Add("password", password);
            body.Add("passwordConfirm", passwordConfirm);

            var url = $"{BasePath()}/confirm-password-reset";
            var result = await client.SendAsync<UserAuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

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

        public async Task<UserAuthModel?> ConfirmVerificationAsync(string token, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", token);

            var url = $"{BasePath()}/confirm-verification";
            var result = await client.SendAsync<UserAuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

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

        public async Task<UserAuthModel?> ConfirmEmailChangeAsync(string emailChangeToken, string userPassword, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", emailChangeToken);
            body.Add("password", userPassword);

            var url = $"{BasePath()}/confirm-email-change";
            var result = await client.SendAsync<UserAuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

            SaveAuthentication(result);

            return result;
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

        private void SaveAuthentication(UserAuthModel? adminAuthModel)
        {
            client.AuthStore.Save(adminAuthModel?.Token, adminAuthModel?.User);
        }

    }
}
