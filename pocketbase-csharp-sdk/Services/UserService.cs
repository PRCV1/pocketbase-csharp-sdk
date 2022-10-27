using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace pocketbase_csharp_sdk.Services
{
    public class UserService : BaseService<UserModel>
    {
        protected override string BasePath => "/api/users";

        private readonly PocketBase client;

        public UserService(PocketBase client) : base(client)
        {
            this.client = client;
        }

        public async Task<AuthMethodsList?> GetAuthenticationMethodsAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath}/auth-methods";
            var response = await client.SendAsync<AuthMethodsList>(url, HttpMethod.Get, headers: headers, query: query, body: body);
            return response;
        }
        
        public async Task<UserAuthModel?> AuthenticateViaEmail(string email, string password, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("email", email);
            body.Add("password", password);

            var url = $"{BasePath}/auth-via-email";
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

            var url = $"{BasePath}/auth-via-oauth2";
            var result = await client.SendAsync<UserAuthModel>(url, HttpMethod.Post, headers: headers, body: body, query: query);

            SaveAuthentication(result);

            return result;
        }

        public async Task<UserAuthModel?> RefreshAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath}/refresh";
            var result = await client.SendAsync<UserAuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

            SaveAuthentication(result);

            return result;
        }

        public async Task RequestPasswordResetAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("email", email);

            var url = $"{BasePath}/request-password-reset";
            await client.SendAsync(url, HttpMethod.Post, headers: headers, query: query, body: body);
        }

        public async Task<UserAuthModel?> ConfirmPasswordResetAsync(string passwordResetToken, string password, string passwordConfirm, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", passwordResetToken);
            body.Add("password", password);
            body.Add("passwordConfirm", passwordConfirm);

            var url = $"{BasePath}/confirm-password-reset";
            var result = await client.SendAsync<UserAuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

            SaveAuthentication(result);

            return result;
        }

        public async Task RequestVerificationAsync(string email, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("email", email);

            var url = $"{BasePath}/request-verification";
            await client.SendAsync(url, HttpMethod.Post, headers: headers, query: query, body: body);
        }

        public async Task<UserAuthModel?> ConfirmVerificationAsync(string token, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", token);

            var url = $"{BasePath}/confirm-verification";
            var result = await client.SendAsync<UserAuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

            SaveAuthentication(result);

            return result;
        }

        public async Task RequestEmailChangeAsync(string newEmail, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("newEmail", newEmail);

            var url = $"{BasePath}/request-email-change";
            await client.SendAsync(url, HttpMethod.Post, headers: headers, query: query, body: body);
        }

        public async Task<UserAuthModel?> ConfirmEmailChangeAsync(string emailChangeToken, string userPassword, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", emailChangeToken);
            body.Add("password", userPassword);

            var url = $"{BasePath}/confirm-email-change";
            var result = await client.SendAsync<UserAuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

            SaveAuthentication(result);

            return result;
        }

        public async Task<IEnumerable<ExternalAuthModel>?> GetExternalAuthenticationMethods(string userId, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath}/{HttpUtility.HtmlEncode(userId)}/confirm-email-change";
            var result = await client.SendAsync<IEnumerable<ExternalAuthModel>>(url, HttpMethod.Get, headers: headers, query: query, body: body);
            return result;
        }

        public async Task UnlinkExternalAuthentication(string userId, string provider, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath}/{HttpUtility.HtmlEncode(userId)}/external-auths/{HttpUtility.HtmlEncode(provider)}";
            await client.SendAsync(url, HttpMethod.Delete, headers: headers, query: query, body: body);
        }

        private void SaveAuthentication(UserAuthModel? adminAuthModel)
        {
            client.AuthStore.Save(adminAuthModel?.Token, adminAuthModel?.User);
        }

    }
}
