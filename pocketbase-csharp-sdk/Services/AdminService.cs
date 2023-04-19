using pocketbase_csharp_sdk.Models.Auth;
using pocketbase_csharp_sdk.Services.Base;

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
