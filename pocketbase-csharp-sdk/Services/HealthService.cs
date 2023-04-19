using pocketbase_csharp_sdk.Models;
using FluentResults;
using pocketbase_csharp_sdk.Services.Base;

namespace pocketbase_csharp_sdk.Services
{
    public class HealthService : BaseService
    {
        private readonly PocketBase _pocketBase;

        protected override string BasePath(string? path = null) => "api/health";

        public HealthService(PocketBase pocketBase)
        {
            this._pocketBase = pocketBase;
        }

        /// <summary>
        /// Returns the health status of the server.
        /// </summary>
        public Task<Result<ApiHealthModel>> CheckHealthAsync()
        {
            return _pocketBase.SendAsync<ApiHealthModel>(BasePath(), HttpMethod.Get);
        }

        /// <summary>
        /// Returns the health status of the server.
        /// </summary>
        public Result<ApiHealthModel> CheckHealth()
        {
            return _pocketBase.Send<ApiHealthModel>(BasePath(), HttpMethod.Get);
        }

    }
}
