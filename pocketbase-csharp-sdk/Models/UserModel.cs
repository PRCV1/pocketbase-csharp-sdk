using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public class UserModel : BaseModel
    {
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public bool? EmailVisibility { get; set; }
        public string? Username { get; set; }
        public bool? Verified { get; set; }
    }
}
