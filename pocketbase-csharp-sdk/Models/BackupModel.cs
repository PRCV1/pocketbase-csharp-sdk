using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public class BackupModel : BaseModel
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("size")]
        public int? Size { get; set; }

        [JsonPropertyName("modified")]
        public DateTime? Modified { get; set; }
    }
}
