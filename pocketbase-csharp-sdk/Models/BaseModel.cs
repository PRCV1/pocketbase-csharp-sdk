using pocketbase_csharp_sdk.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public abstract class BaseModel
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
    }
}
