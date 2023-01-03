using pocketbase_csharp_sdk.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public abstract class BaseAuthService : BaseService
    {
        protected readonly PocketBase client;
        public BaseAuthService(PocketBase client)
        {
            this.client = client;
        }

        public async Task<AuthModel?> AuthenticateWithPassword(string email, string password, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("identity", email);
            body.Add("password", password);

            var url = $"{BasePath()}/auth-with-password";
            var result = await client.SendAsync<AuthModel>(url, HttpMethod.Post, headers: headers, body: body, query: query);

            SaveAuthentication(result);

            return result;
        }

        public async Task<AuthModel?> RefreshAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath()}/auth-refresh";
            var result = await client.SendAsync<AuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

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

        public async Task<AuthModel?> ConfirmPasswordResetAsync(string passwordResetToken, string password, string passwordConfirm, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", passwordResetToken);
            body.Add("password", password);
            body.Add("passwordConfirm", passwordConfirm);

            var url = $"{BasePath()}/confirm-password-reset";
            var result = await client.SendAsync<AuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

            SaveAuthentication(result);

            return result;
        }


        protected void SaveAuthentication(AuthModel? authModel)
        {
            if(authModel?.IsAdmin??false)
                client.AuthStore.Save(authModel?.Token, authModel?.Admin);
            else
                client.AuthStore.Save(authModel?.Token, authModel?.User);
        }
    }
}
