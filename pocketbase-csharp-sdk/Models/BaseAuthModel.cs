using System.Text.Json.Serialization;

namespace pocketbase_csharp_sdk.Models
{
    public class BaseAuthModel : BaseModel, IBaseAuthModel
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        
        [JsonPropertyName("emailVisibility")] 
        public bool? EmailVisibility { get; set; }
        
        [JsonPropertyName("username")]
        public string? Username { get; set; }
        
        [JsonPropertyName("verified")]
        public bool? Verified { get; set; }
    }
}
