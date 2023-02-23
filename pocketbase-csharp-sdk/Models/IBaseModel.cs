using pocketbase_csharp_sdk.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public interface IBaseModel
    {
        [JsonPropertyName("id")]
        string? Id { get; set; }

        [JsonPropertyName("created")]
        [JsonConverter(typeof(DateTimeConverter))]
        DateTime? Created { get; set; }

        [JsonPropertyName("updated")]
        [JsonConverter(typeof(DateTimeConverter))]
        DateTime? Updated { get; set; }

        [JsonPropertyName("collectionId")]
        string? CollectionId { get; set; }

        [JsonPropertyName("collectionName")]
        string? CollectionName { get; set; }
    }
}
