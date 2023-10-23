using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Services.Base;

namespace pocketbase_csharp_sdk.Services
{
    public class AdminService : BaseAuthService<PbAdmin>
    {
        public AdminService(PocketBase pocketBase) : base(pocketBase)
        {
        }

        protected override string GetBasePath()
        {
            return "/api/admins";
        }
    }
}