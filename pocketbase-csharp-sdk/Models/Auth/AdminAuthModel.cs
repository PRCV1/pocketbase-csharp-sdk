using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models.Auth
{
    public class AdminAuthModel
    {
        public string? Token { get; set; }
        public AdminModel? Admin { get; set; }
    }
}
