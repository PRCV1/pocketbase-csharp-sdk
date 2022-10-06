using FluentAssertions;
using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Tests
{
    [TestClass]
    public class AuthStoreTests
    {
        [TestMethod]
        public void Test_Is_Not_Valid_With_Empty_Token()
        {
            AuthStore store = new();

            store.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void Test_Is_Not_Valid_With_Invalid_Token()
        {
            AuthStore store = new();

            store.Save("invalid", null);

            store.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void Test_Is_Not_Valid_With_Expired_Token()
        {
            AuthStore store = new();
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2NDA5OTE2NjF9.TxZjXz_Ks665Hju0FkZSGqHFCYBbgBmMGOLnIzkg9Dg";

            store.Save(token, null);

            store.IsValid.Should().BeFalse();
        }

        [TestMethod]
        public void Test_Is_Valid_With_Valid_Token()
        {
            AuthStore store = new();
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE4OTM0NTI0NjF9.yVr-4JxMz6qUf1MIlGx8iW2ktUrQaFecjY_TMm7Bo4o";

            store.Save(token, null);

            store.IsValid.Should().BeTrue();
        }

        [TestMethod]
        public void Test_Authtoken_Can_be_Read()
        {
            AuthStore store = new();
            string token = "test_token";

            store.Save(token, null);

            store.Token.Should().Be(token);
        }

        [TestMethod]
        public void Test_Model_Can_be_Read()
        {
            AuthStore store = new();
            UserModel model = new UserModel();

            store.Save(null, model);

            store.Model.Should().Be(model);
        }

        [TestMethod]
        public void Test_Store_Can_be_Saved()
        {
            AuthStore store = new();

            string token = "token123";
            UserModel model = new UserModel();

            store.OnChange += (sender, e) =>
            {
                e.Token.Should().Be(token);
                e.Model.Should().Be(model);
            };

            store.Save(token, model);

            store.Model.Should().Be(model);
            store.Token.Should().Be(token);
        }

        [TestMethod]
        public void Test_Store_Can_be_Cleared()
        {
            AuthStore store = new();

            string token = "token123";
            UserModel model = new UserModel();

            store.Save(token, model);

            store.Token.Should().Be(token);
            store.Model.Should().Be(model);

            store.OnChange += (sender, e) =>
            {
                e.Token.Should().BeNull();
                e.Model.Should().BeNull();
            };

            store.Clear();

            store.Token.Should().BeNull();
            store.Model.Should().BeNull();
        }

    }
}
