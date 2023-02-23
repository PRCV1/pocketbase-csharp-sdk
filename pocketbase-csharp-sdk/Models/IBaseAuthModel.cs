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
        string? Email { get; }

        bool? EmailVisibility { get; }

        string? Username { get; }

        bool? Verified { get; }
    }
}
