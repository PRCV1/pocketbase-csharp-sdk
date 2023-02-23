using pocketbase_csharp_sdk.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace pocketbase_csharp_sdk.Models
{
    public abstract class BaseModel : IBaseModel
    {
        [JsonPropertyName("id")]
        public virtual string? Id { get; set; }

        [JsonPropertyName("created")]
        [JsonConverter(typeof(DateTimeConverter))]
        public virtual DateTime? Created { get; set; }

        [JsonPropertyName("updated")]
        [JsonConverter(typeof(DateTimeConverter))]
        public virtual DateTime? Updated { get; set; }

        [JsonPropertyName("collectionId")]
        public virtual string? CollectionId { get; set; }

        [JsonPropertyName("collectionName")]
        public virtual string? CollectionName { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
