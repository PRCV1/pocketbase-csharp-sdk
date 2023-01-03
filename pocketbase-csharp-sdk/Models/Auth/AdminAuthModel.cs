using System.Text.Json.Serialization;

namespace pocketbase_csharp_sdk.Models.Auth
{
    public class AdminAuthModel : AuthModel
    {
        [JsonIgnore]
        public override BaseModel? Model => Admin;


        [JsonPropertyName("admin")]
        public AdminModel? Admin { get; set; }
    }
}
