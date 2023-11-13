using System.Text.Json.Serialization;

namespace pocketbase_csharp_sdk.Models
{
    
    /// <summary>
    /// Used for responses from the AdminService
    /// </summary>
    public class PbAdminAuth
    {
        [JsonPropertyName("token")]
        public string? Token { get; set; }

        [JsonPropertyName("admin")]
        public PbAdmin? Admin { get; set; }
    }
}