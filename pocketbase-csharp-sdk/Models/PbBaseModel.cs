using System.Text.Json.Serialization;
using pocketbase_csharp_sdk.Json;

namespace pocketbase_csharp_sdk.Models
{
    public abstract class PbBaseModel
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        
        [JsonPropertyName("created")]
        [JsonConverter(typeof(PbDateTimeConverter))]
        public DateTime? Created { get; set; }
        
        [JsonPropertyName("updated")]
        [JsonConverter(typeof(PbDateTimeConverter))]
        public DateTime? Updated { get; set; }
    }
}