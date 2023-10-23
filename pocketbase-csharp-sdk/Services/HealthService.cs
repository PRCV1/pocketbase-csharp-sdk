using FluentResults;
using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Services.Base;

namespace pocketbase_csharp_sdk.Services
{
    public class HealthService : BaseCrudService
    {
        private readonly PocketBase _pocketBase;

        public HealthService(PocketBase pocketBase) : base(pocketBase)
        {
            _pocketBase = pocketBase;
        }

        protected override string GetBasePath()
        {
            return "api/health";
        }
        
        public async Task<Result<ApiHealthModel>> CheckHealthAsync()
        {
            var result = await _pocketBase.SendAsync<ApiHealthModel>(GetBasePath(), HttpMethod.Get).ConfigureAwait(false);
            return result;
        }

        public Result<ApiHealthModel> CheckHealth()
        {
            return _pocketBase.Send<ApiHealthModel>(GetBasePath(), HttpMethod.Get);
        }

        
    }
}