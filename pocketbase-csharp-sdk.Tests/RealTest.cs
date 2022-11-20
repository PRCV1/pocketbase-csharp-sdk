using pocketbase_csharp_sdk.Models;
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
            //var refresh = await client.Admin.RefreshAsync();
            //await client.Admin.RequestPasswordResetAsync("test@test.de");
            //await client.Admin.ConfirmPasswordResetAsync("token", "0123456789", "0123456789");

            //var ttest = await client.User.GetAuthenticationMethodsAsync();
            //var test = await client.User.AuthenticateViaEmail("kekw@kekw.com", "0123456789");
            //var test2 = await client.User.AuthenticateViaOAuth2("twitter", "0123456789", "twitter", "google.de");
            //var test3 = await client.User.RefreshAsync();
            //await client.User.RequestPasswordResetAsync("kekw@kekw.com");
            //await client.User.ConfirmPasswordResetAsync("token", "0123456789", "0123456789");
            //await client.User.RequestVerificationAsync("kekw@kekw.com");
            //await client.User.RequestEmailChangeAsync("keko@keko.com");
            //await client.User.ConfirmEmailChangeAsync("token", "0123456789");
            //await client.User.GetExternalAuthenticationMethods("ay9v60tj4rlb4nf");
            //await client.User.UnlinkExternalAuthentication("ay9v60tj4rlb4nf", "twitter");

            //await client.Log.GetRequestAsync("07s3a8j0s2957og");
            //await client.Log.GetRequestsAsync();
            //await client.Log.GetRequestsStatisticsAsync();

            //await client.Settings.GetAllAsync();

            await client.User.GetFullListAsync(5);
            //var lol = await client.User.CreateAsync("pepega@test.de", "0123456789", "0123456789");

            var erster = await client.User.GetOne("5fw837ksugnnt36");

            var nr = new Restaurant()
            {
                Name = "Pepega Neu"
            };

            var lel = await client.Records.ListAsync<Restaurant>("restaurants");
            //var lelo = await client.Records.CreateAsync<Restaurant>("restaurants", nr);
            await client.Records.UpdateAsync<Restaurant>("restaurants", "y65ltbn18v5u4cu", nr);
        }
    }

    class Restaurant : ItemBaseModel
    {
        public string? Name { get; set; }
    }

}
