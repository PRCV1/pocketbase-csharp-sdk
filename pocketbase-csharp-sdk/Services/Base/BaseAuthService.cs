using FluentResults;
using pocketbase_csharp_sdk.Models.Auth;

namespace pocketbase_csharp_sdk.Services.Base
{
    public abstract class BaseAuthService<T> : BaseService
        where T : AuthModel
    {
        protected readonly PocketBase client;
        public BaseAuthService(PocketBase client) : base(client)
        {
            this.client = client;
        }

        /// <summary>
        /// authenticates a user with the API using their email/username and password.
        /// </summary>
        /// <param name="usernameOrEmail">The email/username of the user to authenticate.</param>
        /// <param name="password">The password of the user to authenticate.</param>
        /// <param name="body">The request body to send to the API. Default is null.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>An object of type T containing the authenticated user's information.</returns>
        public async Task<Result<T>> AuthenticateWithPasswordAsync(string usernameOrEmail, string password, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, object> 
            {
                { "identity", usernameOrEmail },
                { "password", password }
            };

            var url = $"{BasePath()}/auth-with-password";
            var result = await client.SendAsync<T>(url, HttpMethod.Post, headers: headers, body: body, query: query, cancellationToken: cancellationToken);

            return SetAndReturn(result);
        }

        /// <summary>
        /// authenticates a user with the API using their email/username and password.
        /// </summary>
        /// <param name="usernameOrEmail">The email/username of the user to authenticate.</param>
        /// <param name="password">The password of the user to authenticate.</param>
        /// <param name="body">The request body to send to the API. Default is null.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>An object of type T containing the authenticated user's information.</returns>
        public Result<T> AuthenticateWithPassword(string usernameOrEmail, string password, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, object> {
                { "identity", usernameOrEmail },
                { "password", password }
            };

            var url = $"{BasePath()}/auth-with-password";
            var result = client.Send<T>(url, HttpMethod.Post, headers: headers, body: body, query: query, cancellationToken: cancellationToken);

            return SetAndReturn(result);
        }

        /// <summary>
        /// refreshes an authenticated user's token
        /// </summary>
        /// <param name="body">The request body to send to the API. Default is null.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>An object of type T containing the authenticated user's updated information.</returns>
        public async Task<Result<T>> RefreshAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/auth-refresh";
            var result = await client.SendAsync<T>(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);

            return SetAndReturn(result);
        }

        /// <summary>
        /// refreshes an authenticated user's token
        /// </summary>
        /// <param name="body">The request body to send to the API. Default is null.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>An object of type T containing the authenticated user's updated information.</returns>
        public Result<T> Refresh(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BasePath()}/auth-refresh";
            var result = client.Send<T>(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);

            return SetAndReturn(result);
        }

        /// <summary>
        /// sends a password reset request for a specific email.
        /// </summary>
        /// <param name="email">The email of the user to send the password reset request to.</param>
        /// <param name="body">The request body to send to the API. Default is null.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        public Task RequestPasswordResetAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("email", email);

            var url = $"{BasePath()}/request-password-reset";
            return client.SendAsync(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// sends a password reset request for a specific email.
        /// </summary>
        /// <param name="email">The email of the user to send the password reset request to.</param>
        /// <param name="body">The request body to send to the API. Default is null.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        public void RequestPasswordReset(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("email", email);

            var url = $"{BasePath()}/request-password-reset";
            client.Send(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// confirms a password reset request using a reset token, and a new password.
        /// </summary>
        /// <param name="passwordResetToken">The password reset token sent to the user's email.</param>
        /// <param name="password">The new password to set for the user.</param>
        /// <param name="passwordConfirm">The confirmation of the new password.</param>
        /// <param name="body">The request body to send to the API. Default is null.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>An object of type T containing the authenticated user's updated information.</returns>
        public async Task<Result<T>> ConfirmPasswordResetAsync(string passwordResetToken, string password, string passwordConfirm, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", passwordResetToken);
            body.Add("password", password);
            body.Add("passwordConfirm", passwordConfirm);

            var url = $"{BasePath()}/confirm-password-reset";
            var result = await client.SendAsync<T>(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);

            return SetAndReturn(result);
        }

        /// <summary>
        /// confirms a password reset request using a reset token, and a new password.
        /// </summary>
        /// <param name="passwordResetToken">The password reset token sent to the user's email.</param>
        /// <param name="password">The new password to set for the user.</param>
        /// <param name="passwordConfirm">The confirmation of the new password.</param>
        /// <param name="body">The request body to send to the API. Default is null.</param>
        /// <param name="query">The query parameters to send to the API. Default is null.</param>
        /// <param name="headers">The headers to send to the API. Default is null.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation. Default is the default cancellation token.</param>
        /// <returns>An object of type T containing the authenticated user's updated information.</returns>
        public Result<T> ConfirmPasswordReset(string passwordResetToken, string password, string passwordConfirm, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", passwordResetToken);
            body.Add("password", password);
            body.Add("passwordConfirm", passwordConfirm);

            var url = $"{BasePath()}/confirm-password-reset";
            var result = client.Send<T>(url, HttpMethod.Post, headers: headers, query: query, body: body, cancellationToken: cancellationToken);

            return SetAndReturn(result);
        }


        protected void SaveAuthentication(T? authModel)
        {
            client.AuthStore.Save(authModel?.Token, authModel?.Model);
        }

        protected Result<T> SetAndReturn(Result<T> result)
        {
            if (result.IsSuccess)
            {
                SaveAuthentication(result.Value);
            }

            return result;
        }
    }
}
