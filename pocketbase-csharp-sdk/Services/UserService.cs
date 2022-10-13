using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public class UserService : BaseService<UserModel>
    {
        protected override string BasePath => "/api/users";

        private readonly PocketBase client;

        public UserService(PocketBase client) : base(client)
        {
            this.client = client;
        }

        public async Task<AuthMethodsList> GetAuthenticationMethodsAsync()
        {
            var response = await client.SendAsync<AuthMethodsList>($"{BasePath}/auth-methods", HttpMethod.Get);
            return response;
        }
        
        public async Task AuthenticateViaEmail(string email, string password, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var enrichedBody = body ?? new Dictionary<string, object>();
            enrichedBody.Add("email", email);
            enrichedBody.Add("password", password);
        }

    }
}
