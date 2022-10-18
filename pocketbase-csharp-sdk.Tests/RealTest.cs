using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Tests
{
    //TODO fix these tests for real unit testing :D

    [TestClass]
    public class RealTest
    {
        [TestMethod]
        public async Task AdminTests()
        {
            var client = new PocketBase("http://127.0.0.1:8090");

            client.AuthStore.OnChange += (s, e) =>
            {
                var lel = e.Token;
            };

            var test = await client.Admin.AuthenticateViaEmail("test@test.de", "0123456789");
            var refresh = await client.Admin.RefreshAsync();
            await client.Admin.RequestPasswordResetAsync("test@test.de");
            await client.Admin.ConfirmPasswordResetAsync("token", "0123456789", "0123456789");
        }
    }
}
