using pocketbase_csharp_sdk.Helper.Extensions;
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

        private readonly CollectionAuthService<UserAuthModel, UserModel> authService;

        public UserService(PocketBase client) : base(client)
        {
            this.client = client;
            this.authService = new CollectionAuthService<UserAuthModel, UserModel>(client, "users");
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

        public override async Task<PagedCollectionModel<UserModel>> ListAsync(int page = 1, int perPage = 30, string? filter = null, string? sort = null)
        {
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };

            var url = $"{BasePath()}/records";
            var response = await client.SendAsync<PagedCollectionModel<UserModel>>(url, HttpMethod.Get, query: query);
            if (response is null)
            {
                throw new ClientException(BasePath());
            }

            return response;
        }

        public override async Task<IEnumerable<UserModel>> GetFullListAsync(int batch = 100, string? filter = null, string? sort = null)
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
        }

        public async Task<UserModel?> GetOne(string id)
        {
            string url = $"{BasePath()}/{UrlEncode(id)}";
            var result = await client.SendAsync<UserModel>(url, HttpMethod.Get);
            return result;
        }

        public async Task<UserModel?> UpdateAsync(string id, string? username = null, string? email = null, bool? emailVisibility = null, string? oldPassword = null, string? password = null, string? passwordConfirm = null, bool? verified = null, string? name = null, object? file = null)
        {
            //TODO File
            Dictionary<string, object> body = new Dictionary<string, object>();
            body.AddIfNotNull("username", username);
            body.AddIfNotNull("email", email);
            body.AddIfNotNull("emailVisibility", emailVisibility);
            body.AddIfNotNull("oldPassword", oldPassword);
            body.AddIfNotNull("password", password);
            body.AddIfNotNull("passwordConfirm", passwordConfirm);
            body.AddIfNotNull("verified", verified);
            body.AddIfNotNull("name", name);

            string url = $"{BasePath()}/records/{UrlEncode(id)}";
            var result = await client.SendAsync<UserModel>(url, HttpMethod.Patch, body: body);
            return result;
        }

        public Task<AuthMethodsList?> GetAuthenticationMethodsAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            return authService.GetAuthenticationMethodsAsync(body, query, headers);
        }

        public Task<UserAuthModel?> AuthenticateWithPassword(string email, string password, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            return authService.AuthenticateWithPasswordAsync(email, password, body, query, headers);
        }

        public Task<UserAuthModel?> AuthenticateViaOAuth2(string provider, string code, string codeVerifier, string redirectUrl, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            return authService.AuthenticateViaOAuth2Async(provider, code, codeVerifier, redirectUrl, body, query, headers);
        }

        public Task<UserAuthModel?> RefreshAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            return authService.RefreshAsync(body, query, headers);
        }

        public Task RequestPasswordResetAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            return authService.RequestPasswordResetAsync(email, body, query, headers);
        }

        public Task<UserAuthModel?> ConfirmPasswordResetAsync(string passwordResetToken, string password, string passwordConfirm, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            return ConfirmPasswordResetAsync(passwordResetToken, password, passwordConfirm, body, query, headers);
        }

        public Task RequestVerificationAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            return authService.RequestVerificationAsync(email, body, query, headers);
        }

        public Task<UserAuthModel?> ConfirmVerificationAsync(string token, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            return authService.ConfirmVerificationAsync(token, body, query, headers);
        }

        public Task RequestEmailChangeAsync(string newEmail, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            return authService.RequestEmailChangeAsync(newEmail, body, query, headers);
        }

        public Task<UserAuthModel?> ConfirmEmailChangeAsync(string emailChangeToken, string userPassword, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            return authService.ConfirmEmailChangeAsync(emailChangeToken, userPassword, body, query, headers);
        }

        public Task<IEnumerable<ExternalAuthModel>?> GetExternalAuthenticationMethods(string userId, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            return authService.GetExternalAuthenticationMethodsAsync(userId, query, headers);
        }

        public Task UnlinkExternalAuthentication(string userId, string provider, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            return authService.UnlinkExternalAuthenticationAsync(userId, provider, body, query, headers);
        }

    }
}
