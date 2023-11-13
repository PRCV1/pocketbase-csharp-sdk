using pocketbase_csharp_sdk.Models;

namespace pocketbase_csharp_sdk.Services.Base
{
    public abstract class BaseAuthService<T> : BaseCrudService where T : PbAdminAuth
    {
        private readonly PocketBase _pocketBase;

        public BaseAuthService(PocketBase pocketBase) : base(pocketBase)
        {
            _pocketBase = pocketBase;
        }

        protected void HandleResponse(T? response)
        {
            if (response is null || string.IsNullOrWhiteSpace(response.Token) || response.Admin is null)
            {
                return;
            }
            
            _pocketBase.AuthStore.Save(response.Token, response.Admin);
        }
        
        public async Task<T> AuthenticateWithPasswordAsync(string usernameOrEmail, string password)
        {
            var url = $"{GetBasePath()}/auth-with-password";

            Dictionary<string, object?> body = new Dictionary<string, object?>() {
                { "identity", usernameOrEmail },
                { "password", password }
            };

            var response = await _pocketBase.SendAsync<T>(url, HttpMethod.Post, body: body);
            HandleResponse(response);
            return response;
        }

        public async Task<T> RefreshAuthenticationAsync()
        {
            var url = $"{GetBasePath()}/auth-refresh";
            var response = await _pocketBase.SendAsync<T>(url, HttpMethod.Post);
            HandleResponse(response);
            return response;
        }

        public async Task RequestPasswordResetAsync(string email)
        {
            var url = $"{GetBasePath()}/request-password-reset";
            
            Dictionary<string, object?> body = new Dictionary<string, object?>() {
                { "email", email },
            };
            
            await _pocketBase.SendAsync(url, HttpMethod.Post, body: body);
        }

        public async Task ConfirmPasswordResetAsync(string token, string password, string passwordConfirm)
        {
            var url = $"{GetBasePath()}/request-password-reset";
            
            Dictionary<string, object?> body = new Dictionary<string, object?>() {
                { "token", token },
                { "password", password },
                { "passwordConfirm", passwordConfirm },
            };
            
            await _pocketBase.SendAsync(url, HttpMethod.Post, body: body);
        }
        
    }
}