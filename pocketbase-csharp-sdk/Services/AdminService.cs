using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public class AdminService : BaseAuthService<AdminAuthModel>
    {

        protected override string BasePath(string? url = null) => "/api/admins";

        public AdminService(PocketBase client) : base(client)
        {
        }

    }
}
