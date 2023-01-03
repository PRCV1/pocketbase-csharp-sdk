using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public class BaseAuthModel : BaseModel
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
