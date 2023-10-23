using System.Text.Json.Serialization;

namespace pocketbase_csharp_sdk.Models
{
    public class PbAdmin : PbBaseModel
    {
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
        
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}