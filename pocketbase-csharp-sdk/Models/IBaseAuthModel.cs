using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public interface IBaseAuthModel : IBaseModel
    {
        [JsonPropertyName("email")]
        string? Email { get; set; }

        [JsonPropertyName("emailVisibility")]
        bool? EmailVisibility { get; set; }

        [JsonPropertyName("username")]
        string? Username { get; set; }

        [JsonPropertyName("verified")]
        bool? Verified { get; set; }
    }
}
