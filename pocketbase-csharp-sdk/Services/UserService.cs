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

        /// <summary>
        /// Asynchronously creates a new user.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="passwordConfirm">The confirmation of the password.</param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns>A task that returns the created `UserModel`.</returns>
        /// <exception cref="ClientException">Thrown if the client fails to send the request or receive a response.</exception>
        public async Task<UserModel> CreateAsync(string email, string password, string passwordConfirm, CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> body = new()
            {
                { "email", email },
                { "password", password },
                { "passwordConfirm", passwordConfirm },
            };

            var response = await client.SendAsync<UserModel>(BasePath(), HttpMethod.Post, body: body, cancellationToken: cancellationToken);
            if (response is null)
            {
                throw new ClientException(BasePath());
            }

            return response;
        }

        /// <summary>
        /// Asynchronously lists all users in a paginated manner.
        /// </summary>
        /// <param name="page">The page number of the results to retrieve (default is 1).</param>
        /// <param name="perPage">The number of results to retrieve per page (default is 30).</param>
        /// <param name="filter">The filter to apply to the results (optional).</param>
        /// <param name="sort">The sort order to apply to the results (optional).</param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns>A task that returns a `PagedCollectionModel` of `UserModel`.</returns>
        /// <exception cref="ClientException">Thrown if the client fails to send the request or receive a response.</exception>
        public override async Task<PagedCollectionModel<UserModel>> ListAsync(int page = 1, int perPage = 30, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };

            var url = $"{BasePath()}/records";
            var response = await client.SendAsync<PagedCollectionModel<UserModel>>(url, HttpMethod.Get, query: query, cancellationToken: cancellationToken);
            if (response is null)
            {
                throw new ClientException(BasePath());
            }

            return response;
        }

        /// <summary>
        /// Asynchronously lists all users.
        /// </summary>
        /// <param name="page">The page number of the results to retrieve (default is 1).</param>
        /// <param name="perPage">The number of results to retrieve per page (default is 30).</param>
        /// <param name="filter">The filter to apply to the results (optional).</param>
        /// <param name="sort">The sort order to apply to the results (optional).</param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns>A task that returns a `PagedCollectionModel` of `UserModel`.</returns>
        /// <exception cref="ClientException">Thrown if the client fails to send the request or receive a response.</exception>
        public override async Task<IEnumerable<UserModel>> GetFullListAsync(int batch = 100, string? filter = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            List<UserModel> result = new();
            int currentPage = 1;
            PagedCollectionModel<UserModel> lastResponse;
            do
            {
                lastResponse = await ListAsync(currentPage, perPage: batch, filter: filter, sort: sort, cancellationToken: cancellationToken);
                if (lastResponse is not null && lastResponse.Items is not null)
                {
                    result.AddRange(lastResponse.Items);
                }
                currentPage++;
            } while (lastResponse?.Items?.Length > 0 && lastResponse?.TotalItems > result.Count);

            return result;
        }

        /// <summary>
        /// Gets the specific user from the id
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns>The cancellation token for the request.</returns>
        public Task<UserModel?> GetOneAsync(string id, CancellationToken cancellationToken = default)
        {
            string url = $"{BasePath()}/{UrlEncode(id)}";
            return client.SendAsync<UserModel>(url, HttpMethod.Get, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Updates a users info. Only non-null values will be updated
        /// </summary>
        /// <param name="id">the id from the user to update</param>
        /// <param name="username">The username of the auth record.</param>
        /// <param name="email">The auth record email address. </param>
        /// <param name="emailVisibility">Whether to show/hide the auth record email when fetching the record data.</param>
        /// <param name="oldPassword">Old auth record password.</param>
        /// <param name="password">New auth record password.</param>
        /// <param name="passwordConfirm">New auth record password confirmation.</param>
        /// <param name="verified">Indicates whether the auth record is verified or not. </param>
        /// <param name="file"></param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns></returns>
        public Task<UserModel?> UpdateAsync(string id, string? username = null, string? email = null, bool? emailVisibility = null, string? oldPassword = null, string? password = null, string? passwordConfirm = null, bool? verified = null, object? file = null, CancellationToken cancellationToken = default)
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

            string url = $"{BasePath()}/records/{UrlEncode(id)}";
            return client.SendAsync<UserModel>(url, HttpMethod.Patch, body: body, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Returns all available application authentication methods
        /// </summary>
        /// <param name="query"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns></returns>
        public Task<AuthMethodsList?> GetAuthenticationMethodsAsync(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.GetAuthenticationMethodsAsync(query, headers, cancellationToken);
        }

        /// <summary>
        /// authenticates a user with the API using their email/username and password.
        /// </summary>
        /// <param name="usernameOrEmail">The email/username of the user to authenticate.</param>
        /// <param name="password">The password of the user to authenticate.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>An object of type T containing the authenticated user's information.</returns>
        public Task<UserAuthModel?> AuthenticateWithPasswordAsync(string usernameOrEmail, string password, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.AuthenticateWithPasswordAsync(usernameOrEmail, password, query, headers, cancellationToken);
        }

        public Task<UserAuthModel?> AuthenticateViaOAuth2Async(string provider, string code, string codeVerifier, string redirectUrl, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.AuthenticateViaOAuth2Async(provider, code, codeVerifier, redirectUrl, body, query, headers, cancellationToken);
        }

        public Task<UserAuthModel?> RefreshAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.RefreshAsync(body, query, headers, cancellationToken);
        }

        public Task RequestPasswordResetAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.RequestPasswordResetAsync(email, body, query, headers, cancellationToken);
        }

        public Task<UserAuthModel?> ConfirmPasswordResetAsync(string passwordResetToken, string password, string passwordConfirm, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.ConfirmPasswordResetAsync(passwordResetToken, password, passwordConfirm, body, query, headers, cancellationToken);
        }

        public Task RequestVerificationAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.RequestVerificationAsync(email, body, query, headers, cancellationToken);
        }

        public Task<UserAuthModel?> ConfirmVerificationAsync(string token, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.ConfirmVerificationAsync(token, body, query, headers, cancellationToken);
        }

        public Task RequestEmailChangeAsync(string newEmail, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.RequestEmailChangeAsync(newEmail, body, query, headers, cancellationToken);
        }

        public Task<UserAuthModel?> ConfirmEmailChangeAsync(string emailChangeToken, string userPassword, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.ConfirmEmailChangeAsync(emailChangeToken, userPassword, body, query, headers, cancellationToken);
        }

        public Task<IEnumerable<ExternalAuthModel>?> GetExternalAuthenticationMethods(string userId, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.GetExternalAuthenticationMethodsAsync(userId, query, headers, cancellationToken);
        }

        public Task UnlinkExternalAuthentication(string userId, string provider, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.UnlinkExternalAuthenticationAsync(userId, provider, body, query, headers, cancellationToken);
        }

    }
}
