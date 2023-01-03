using System.Text.Json.Serialization;

namespace pocketbase_csharp_sdk.Models.Auth
{
    public abstract class AuthModel
    {
        public string? Token { get; set; }

        [JsonIgnore]
        public abstract BaseModel? Model { get; }
    }
}
