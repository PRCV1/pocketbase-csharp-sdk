using pocketbase_csharp_sdk.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace pocketbase_csharp_sdk.Models
{
    public class ItemBaseModel
    {
        [JsonPropertyName("@collectionId")]
        public string? CollectionId { get; set; }

        [JsonPropertyName("@collectionName")]
        public string? CollectionName { get; set; }

        [JsonPropertyName("@expand")]
        public JsonObject? Expand { get; set; }

        public string? Id { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Created { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Updated { get; set; }
    }

}
