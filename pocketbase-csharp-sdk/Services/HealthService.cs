using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public class HealthService : BaseService
    {
        private readonly PocketBase pocketBase;

        protected override string BasePath(string? path = null) => "api/health";

        public HealthService(PocketBase pocketBase)
        {
            this.pocketBase = pocketBase;
        }

        /// <summary>
        /// Returns the health status of the server.
        /// </summary>
        public Task<ApiHealthModel?> CheckHealthAsync()
        {
            return pocketBase.SendAsync<ApiHealthModel>(BasePath(), HttpMethod.Get);
        }
        
    }
}
