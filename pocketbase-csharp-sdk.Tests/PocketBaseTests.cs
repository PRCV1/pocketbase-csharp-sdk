using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using pocketbase_csharp_sdk.Helper;

namespace pocketbase_csharp_sdk.Tests
{
    [TestClass]
    public class PocketBaseTests
    {

        [TestMethod]
        [DataRow("https://example.com/", "test")]
        [DataRow("https://example.com/", "/test")]
        [DataRow("https://example.com", "test")]
        [DataRow("https://example.com", "/test")]
        public void Test_BaseUrls(string baseUrl, string url)
        {
            var client = new UrlBuilder(baseUrl);

            var fullUrl = client.BuildUrl(url, new PbListQueryParams()).ToString();

            fullUrl.Should().Be("https://example.com/test");
        }

        [TestMethod]
        [DataRow("test")]
        [DataRow("/test")]
        public void Test_Relative_Urls(string relativeUrl)
        {
            var client = new UrlBuilder("/api");

            var url = client.BuildUrl(relativeUrl, new PbListQueryParams()).ToString();

            url.Should().Be("/api/test");
        }

        // [TestMethod]
        // public void Test_With_Query_Paramters()
        // {
        //     var client = new UrlBuilder("https://example.com");
        //
        //     Dictionary<string, object?> parameters = new()
        //     {
        //         { "a", null },
        //         { "b", 123 },
        //         { "c", "123" },
        //         { "d", new object?[] { "1", 2, null } },
        //         { "@encodeA", "@encodeB" },
        //     };
        //     var url = client.BuildUrl("/test", parameters).ToString();
        //
        //     url.Should().Be("https://example.com/test?b=123&c=123&d=1&d=2&%40encodeA=%40encodeB");
        // }

    }
}