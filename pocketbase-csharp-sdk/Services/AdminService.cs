using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public class AdminService : BaseService<AdminModel>
    {

        protected override string BasePath => "/api/admins";

        private readonly PocketBase client;
        public AdminService(PocketBase client) : base(client)
        {
            this.client = client;
        }

        public async Task<AdminAuthModel> AuthenticateViaEmail(string email, string password, IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var enrichedBody = body ?? new Dictionary<string, object>();
            enrichedBody.Add("email", email);
            enrichedBody.Add("password", password);

            var enrichedHeaders = headers ?? new Dictionary<string, string>();
            //enrichedHeaders.Add("Authorization", "");

            var url = $"{BasePath}/auth-via-email";
            var result = await client.SendAsync<AdminAuthModel>(url, HttpMethod.Post, headers: enrichedHeaders, query: query, body: enrichedBody);
            SaveAuthentication(result);
            return default;
        }

        public async Task<AdminAuthModel> RefreshAsync(IDictionary<string, object>? body = null, IDictionary<string, object?>? query = null, IDictionary<string, string>? headers = null)
        {
            var url = $"{BasePath}/refresh";
            var result = await client.SendAsync<AdminAuthModel>(url, HttpMethod.Post, body: body, query: query, headers: headers);
            SaveAuthentication(result);
            return default;
        }

        private void SaveAuthentication(AdminAuthModel adminAuthModel)
        {
            client.AuthStore.Save(adminAuthModel.Token, adminAuthModel.Admin);
        }

    }
}
