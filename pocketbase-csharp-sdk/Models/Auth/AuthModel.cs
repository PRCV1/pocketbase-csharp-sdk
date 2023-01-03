using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models.Auth
{
    public class AuthModel
    {
        public string? Token { get; set; }

        [JsonPropertyName("record")]
        public BaseAuthModel? User { get; set; }

        [JsonPropertyName("admin")]
        public AdminModel? Admin { get; set; }

        public IDictionary<string, object?>? meta { get; set; }

        [JsonIgnore]
        public bool IsAdmin => Admin != null;
    }
}
