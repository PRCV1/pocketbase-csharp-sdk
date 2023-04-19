using pocketbase_csharp_sdk.Enum;
using FluentResults;
using pocketbase_csharp_sdk.Services.Base;

namespace pocketbase_csharp_sdk.Services
{
    public class RecordService : BaseSubCrudService
    {

        protected override string BasePath(string? path = null)
        {
            var encoded = UrlEncode(path);
            return $"/api/collections/{encoded}/records";
        }

        private readonly PocketBase _client;

        public RecordService(PocketBase client) : base(client)
        {
            this._client = client;
        }

        private Uri GetFileUrl(string sub, string recordId, string fileName, IDictionary<string, object?>? query = null)
        {
            var url = $"api/files/{sub}/{UrlEncode(recordId)}/{fileName}";
            return _client.BuildUrl(url, query);
        }

        public Task<Result<Stream>> DownloadFileAsync(string collectionId, string recordId, string fileName, ThumbFormat? thumbFormat = null, CancellationToken cancellationToken = default)
        {
            var url = $"api/files/{UrlEncode(collectionId)}/{UrlEncode(recordId)}/{fileName}";

            //TODO find out how the specify the actual resolution to resize
            var query = new Dictionary<string, object?>()
            {
                { "thumb", ThumbFormatHelper.GetNameForQuery(thumbFormat) }
            };

            return _client.GetStreamAsync(url, query, cancellationToken);
        }

    }
}
