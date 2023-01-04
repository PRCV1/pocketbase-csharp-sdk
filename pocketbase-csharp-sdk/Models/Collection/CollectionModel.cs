using pocketbase_csharp_sdk.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models.Collection
{
    public class CollectionModel
    {
        public string? Id { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Created { get; set; }
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Updated { get; set; }

        public string? Name { get; set; }
        public bool? System { get; set; }
        public string? Type { get; set; }

        public string? ListRule { get; set; }
        public string? ViewRule { get; set; }
        public string? CreateRule { get; set; }
        public string? UpdateRule { get; set; }
        public string? DeleteRule { get; set; }

        public CollectionOptionsModel? Options { get; set; }
        public IEnumerable<SchemaFieldModel>? Schema { get; set; }
    }
}
