using System.Text.Json.Serialization;

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
