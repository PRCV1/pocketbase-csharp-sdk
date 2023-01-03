using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public class AdminModel : BaseModel
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("avatar")]
        public int? Avatar { get; set; }
    }
}
