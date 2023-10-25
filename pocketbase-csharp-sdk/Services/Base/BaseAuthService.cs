namespace pocketbase_csharp_sdk.Services.Base
{
    public abstract class BaseAuthService<T> : BaseCrudService
    {
        private readonly PocketBase _pocketBase;

        public BaseAuthService(PocketBase pocketBase) : base(pocketBase)
        {
            _pocketBase = pocketBase;
        }

        public async Task<T> AuthenticateWithPasswordAsync(string usernameOrEmail, string password)
        {
            var url = $"{GetBasePath() + "auth-with-password"}";
            var response = await _pocketBase.SendAsync<T>(url, HttpMethod.Post);
            return response;
        }
        
    }
}