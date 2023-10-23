using FluentResults;
using pocketbase_csharp_sdk.Helper;
using pocketbase_csharp_sdk.Models.Log;
using pocketbase_csharp_sdk.Services.Base;

namespace pocketbase_csharp_sdk.Services
{
    public class LogService : BaseCrudService
    {
        private readonly PocketBase _pocketBase;

        public LogService(PocketBase pocketBase) : base(pocketBase)
        {
            _pocketBase = pocketBase;
        }

        protected override string GetBasePath()
        {
            return "api/logs/requests";
        }

        public async Task<Result<LogRequestModel>> GetGetRequestsListAsync(PbListQueryParams? queryParams = null, IDictionary<string, string>? headers = null)
        {
            var response = await _pocketBase.SendAsync<LogRequestModel>(GetBasePath(), HttpMethod.Get, queryParams, headers).ConfigureAwait(false);
            return response;
        }
        
    }
}