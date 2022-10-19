using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models.Auth
{
    public class UserAuthModel
    {
        public string? Token { get; set; }
        public UserModel? User  { get; set; }
        public IDictionary<string, object?>? meta { get; set; }
    }
}
