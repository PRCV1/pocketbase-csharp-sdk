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
using FluentResults;
using pocketbase_csharp_sdk.Services.Base;

namespace pocketbase_csharp_sdk.Services
{
    public class UserService : BaseCrudService<UserModel>
    {
        protected override string BasePath(string? url = null) => "/api/collections/users/records";

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
        public Task<Result<UserModel>> CreateAsync(string email, string password, string passwordConfirm, CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> body = new()
            {
                { "email", email },
                { "password", password },
                { "passwordConfirm", passwordConfirm },
            };

            return client.SendAsync<UserModel>(BasePath(), HttpMethod.Post, body: body, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// creates a new user.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="passwordConfirm">The confirmation of the password.</param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns>A task that returns the created `UserModel`.</returns>
        /// <exception cref="ClientException">Thrown if the client fails to send the request or receive a response.</exception>
        public Result<UserModel> Create(string email, string password, string passwordConfirm, CancellationToken cancellationToken = default)
        {
            Dictionary<string, object> body = new()
            {
                { "email", email },
                { "password", password },
                { "passwordConfirm", passwordConfirm },
            };

            return client.Send<UserModel>(BasePath(), HttpMethod.Post, body: body, cancellationToken: cancellationToken);
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
        public Task<Result<UserModel>> UpdateAsync(string id, string? username = null, string? email = null, bool? emailVisibility = null, string? oldPassword = null, string? password = null, string? passwordConfirm = null, bool? verified = null, object? file = null, CancellationToken cancellationToken = default)
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
        public Result<UserModel> Update(string id, string? username = null, string? email = null, bool? emailVisibility = null, string? oldPassword = null, string? password = null, string? passwordConfirm = null, bool? verified = null, object? file = null, CancellationToken cancellationToken = default)
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
            return client.Send<UserModel>(url, HttpMethod.Patch, body: body, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Returns all available application authentication methods
        /// </summary>
        /// <param name="query"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns></returns>
        public Task<Result<AuthMethodsList>> GetAuthenticationMethodsAsync(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.GetAuthenticationMethodsAsync(query, headers, cancellationToken);
        }

        /// <summary>
        /// Returns all available application authentication methods
        /// </summary>
        /// <param name="query"></param>
        /// <param name="headers"></param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns></returns>
        public Result<AuthMethodsList> GetAuthenticationMethods(IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.GetAuthenticationMethods(query, headers, cancellationToken);
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
        public Task<Result<UserAuthModel>> AuthenticateWithPasswordAsync(string usernameOrEmail, string password, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.AuthenticateWithPasswordAsync(usernameOrEmail, password, query, headers, cancellationToken);
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
        public Result<UserAuthModel> AuthenticateWithPassword(string usernameOrEmail, string password, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.AuthenticateWithPassword(usernameOrEmail, password, query, headers, cancellationToken);
        }

        public Task<Result<UserAuthModel>> AuthenticateViaOAuth2Async(string provider, string code, string codeVerifier, string redirectUrl, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.AuthenticateViaOAuth2Async(provider, code, codeVerifier, redirectUrl, body, query, headers, cancellationToken);
        }

        public Result<UserAuthModel> AuthenticateViaOAuth2(string provider, string code, string codeVerifier, string redirectUrl, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.AuthenticateViaOAuth2(provider, code, codeVerifier, redirectUrl, body, query, headers, cancellationToken);
        }

        public Task<Result<UserAuthModel>> RefreshAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.RefreshAsync(body, query, headers, cancellationToken);
        }

        public Result<UserAuthModel> Refresh(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.Refresh(body, query, headers, cancellationToken);
        }

        public Task RequestPasswordResetAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.RequestPasswordResetAsync(email, body, query, headers, cancellationToken);
        }

        public void RequestPasswordReset(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            authService.RequestPasswordReset(email, body, query, headers, cancellationToken);
        }

        public Task<Result<UserAuthModel>> ConfirmPasswordResetAsync(string passwordResetToken, string password, string passwordConfirm, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.ConfirmPasswordResetAsync(passwordResetToken, password, passwordConfirm, body, query, headers, cancellationToken);
        }

        public Result<UserAuthModel> ConfirmPasswordReset(string passwordResetToken, string password, string passwordConfirm, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.ConfirmPasswordReset(passwordResetToken, password, passwordConfirm, body, query, headers, cancellationToken);
        }

        public Task RequestVerificationAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.RequestVerificationAsync(email, body, query, headers, cancellationToken);
        }

        public void RequestVerification(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            authService.RequestVerification(email, body, query, headers, cancellationToken);
        }

        public Task<Result<UserAuthModel>> ConfirmVerificationAsync(string token, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.ConfirmVerificationAsync(token, body, query, headers, cancellationToken);
        }

        public Result<UserAuthModel> ConfirmVerification(string token, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.ConfirmVerification(token, body, query, headers, cancellationToken);
        }

        public Task RequestEmailChangeAsync(string newEmail, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.RequestEmailChangeAsync(newEmail, body, query, headers, cancellationToken);
        }

        public void RequestEmailChange(string newEmail, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            authService.RequestEmailChange(newEmail, body, query, headers, cancellationToken);
        }

        public Task<Result<UserAuthModel>> ConfirmEmailChangeAsync(string emailChangeToken, string userPassword, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.ConfirmEmailChangeAsync(emailChangeToken, userPassword, body, query, headers, cancellationToken);
        }

        public Result<UserAuthModel> ConfirmEmailChange(string emailChangeToken, string userPassword, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.ConfirmEmailChange(emailChangeToken, userPassword, body, query, headers, cancellationToken);
        }

        public Task<Result<IEnumerable<ExternalAuthModel>>> GetExternalAuthenticationMethodsAsync(string userId, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.GetExternalAuthenticationMethodsAsync(userId, query, headers, cancellationToken);
        }

        public Result<IEnumerable<ExternalAuthModel>> GetExternalAuthenticationMethods(string userId, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.GetExternalAuthenticationMethods(userId, query, headers, cancellationToken);
        }

        public Task UnlinkExternalAuthenticationAsync(string userId, string provider, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            return authService.UnlinkExternalAuthenticationAsync(userId, provider, body, query, headers, cancellationToken);
        }

        public void UnlinkExternalAuthentication(string userId, string provider, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            authService.UnlinkExternalAuthentication(userId, provider, body, query, headers, cancellationToken);
        }

    }
}
