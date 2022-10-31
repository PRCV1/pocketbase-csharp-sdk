using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public class RecordService : BaseCrudService
    {
        private readonly PocketBase client;

        protected override string BasePath => throw new NotImplementedException();

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
