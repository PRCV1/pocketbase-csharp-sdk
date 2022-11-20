using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace pocketbase_csharp_sdk.Services
{
    public class RecordService : BaseSubCrudService
    {

        protected override string BasePath(string? path = null)
        {
            var encoded = HttpUtility.UrlEncode(path);
            return $"/api/collections/{encoded}/records";
        }

        private readonly PocketBase client;

        public RecordService(PocketBase client) : base(client)
        {
            this.client = client;
        }

        public Uri GetFileUrl(string fileName, IDictionary<string, object?>? query = null)
        {
            //TODO
            return client.BuildUrl("", query);
        }

        
    }
}
