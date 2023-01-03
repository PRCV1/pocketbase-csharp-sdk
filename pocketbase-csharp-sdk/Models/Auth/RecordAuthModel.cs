using System.Text.Json.Serialization;

namespace pocketbase_csharp_sdk.Models.Auth
{
    public class RecordAuthModel<T> : AuthModel where T : BaseAuthModel
    {
        [JsonIgnore]
        public override BaseModel? Model => Record;

        [JsonPropertyName("record")]
        public T? Record { get; set; }

        public IDictionary<string, object?>? meta { get; set; }
    }
}
