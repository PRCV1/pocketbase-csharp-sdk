using pocketbase_csharp_sdk.Models;
using System.Text.Json.Serialization;

namespace Example
{
    public class Users2 : BaseAuthModel
    {
        [JsonPropertyName("public_name")]
        public string? PublicName { get; set; }
    }

}
