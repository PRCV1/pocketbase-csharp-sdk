using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public class AdminService : BaseService
    {

        protected override string BasePath(string? url = null) => "/api/admins";

        private readonly PocketBase client;
        public AdminService(PocketBase client)
        {
            this.client = client;
        }

        public async Task<AdminAuthModel?> AuthenticateViaEmail(string email, string password, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("email", email);
            body.Add("password", password);

            var url = $"{BasePath()}/auth-via-email";
            var result = await client.SendAsync<AdminAuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

            SaveAuthentication(result);

            return result;
        }

        public async Task<AdminAuthModel?> RefreshAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath()}/refresh";
            var result = await client.SendAsync<AdminAuthModel>(url, HttpMethod.Post, body: body, query: query, headers: headers);

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

        public async Task<AdminAuthModel?> ConfirmPasswordResetAsync(string passwordResetToken, string password, string passwordConfirm, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", passwordResetToken);
            body.Add("password", password);
            body.Add("passwordConfirm", passwordConfirm);

            var url = $"{BasePath()}/confirm-password-reset";
            var result = await client.SendAsync<AdminAuthModel>(url, HttpMethod.Post, headers: headers, query: query, body: body);

            SaveAuthentication(result);

            return result;
        }

        private void SaveAuthentication(AdminAuthModel? adminAuthModel)
        {
            client.AuthStore.Save(adminAuthModel?.Token, adminAuthModel?.Admin);
        }

    }
}
