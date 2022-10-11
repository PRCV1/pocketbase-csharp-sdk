using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public class AdminAuthModel
    {
        private readonly string token;
        private readonly AdminModel admin;

        public AdminAuthModel(string token, AdminModel admin)
        {
            this.token = token;
            this.admin = admin;
        }
    }
}
