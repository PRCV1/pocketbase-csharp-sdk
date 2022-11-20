using pocketbase_csharp_sdk.Models.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public class CollectionService : BaseCrudService<CollectionModel>
    {
        private readonly PocketBase client;

        protected override string BasePath(string? url = null) => "/api/collections";

        public CollectionService(PocketBase client) : base(client)
        {
            this.client = client;
        }

        public async Task ImportAsync(IEnumerable<CollectionModel> collections,bool deleteMissing = false, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            body ??= new Dictionary<string, object>();
            body.Add("collections", collections);
            body.Add("deleteMissing", deleteMissing);

            var url = $"{BasePath()}/import";
            await client.SendAsync(url, HttpMethod.Put, headers: headers, query: query, body: body);
        }

    }
}
