using pocketbase_csharp_sdk.Models.Auth;

namespace pocketbase_csharp_sdk.Services
{
    public abstract class BaseAuthService<T> : BaseService
        where T : AuthModel
    {
        protected readonly PocketBase client;
        public BaseAuthService(PocketBase client)
        {
            this.client = client;
        }

        public async Task<T?> AuthenticateWithPasswordAsync(string email, string password, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("identity", email);
            body.Add("password", password);

            var url = $"{BasePath()}/auth-with-password";
            var result = await client.SendAsync<T>(url, HttpMethod.Post, headers: headers, body: body, query: query);

            SaveAuthentication(result);

            return result;
        }

        public async Task<T?> RefreshAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath()}/auth-refresh";
            var result = await client.SendAsync<T>(url, HttpMethod.Post, headers: headers, query: query, body: body);

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

        public async Task<T?> ConfirmPasswordResetAsync(string passwordResetToken, string password, string passwordConfirm, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("token", passwordResetToken);
            body.Add("password", password);
            body.Add("passwordConfirm", passwordConfirm);

            var url = $"{BasePath()}/confirm-password-reset";
            var result = await client.SendAsync<T>(url, HttpMethod.Post, headers: headers, query: query, body: body);

            SaveAuthentication(result);

            return result;
        }


        protected void SaveAuthentication(T? authModel)
        {
            client.AuthStore.Save(authModel?.Token, authModel?.Model);
        }
    }
}
