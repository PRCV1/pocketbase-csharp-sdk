using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models.Log
{
    public class AuthMethodsList
    {
        public bool? UsernamePassword { get; set; }
        public bool? EmailPassword { get; set; }
        public IEnumerable<AuthMethodProvider>? AuthProviders { get; set; }
    }
}
