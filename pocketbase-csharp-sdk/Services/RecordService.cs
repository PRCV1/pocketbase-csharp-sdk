using pocketbase_csharp_sdk.Enum;
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

        private Uri GetFileUrl(string sub, string recordId, string fileName, IDictionary<string, object?>? query = null)
        {
            var url = $"api/files/{sub}/{HttpUtility.UrlEncode(recordId)}/{fileName}";
            return client.BuildUrl(url, query);
        }

        public Task<Stream> DownloadFileAsync(string sub, string recordId, string fileName, ThumbFormat? thumbFormat = null)
        {
            //TODO find out how the specify the actual resolution to resize
            var query = new Dictionary<string, object?>() 
            {
                { "thumb", ThumbFormatHelper.GetNameForQuery(thumbFormat) }
            };
            
            var fileUrl = GetFileUrl(sub, recordId, fileName, query);
            return client._httpClient.GetStreamAsync(fileUrl);
        }
        
    }
}
