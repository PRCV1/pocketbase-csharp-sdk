using FluentAssertions;
using Moq;
using Moq.Protected;
using RichardSzalay.MockHttp;
using System.Net;

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
            var client = new PocketBase(baseUrl);

            var fullUrl = client.BuildUrl(url).ToString();

            fullUrl.Should().Be("https://example.com/test");
        }

        [TestMethod]
        [DataRow("test")]
        [DataRow("/test")]
        public void Test_Relative_Urls(string relativeUrl)
        {
            var client = new PocketBase("/api");

            var url = client.BuildUrl(relativeUrl).ToString();

            url.Should().Be("/api/test");
        }

        [TestMethod]
        public void Test_With_Query_Paramters()
        {
            var client = new PocketBase("https://example.com");

            Dictionary<string, object?> parameters = new()
            {
                { "a", null },
                { "b", 123 },
                { "c", "123" },
                { "d", new object?[] { "1", 2, null } },
                { "@encodeA", "@encodeB" },
            };
            var url = client.BuildUrl("/test", parameters).ToString();

            url.Should().Be("https://example.com/test?b=123&c=123&d=1&d=2&%40encodeA=%40encodeB");
        }

        [TestMethod]
        public async Task Test_Check_Request_Data_Json()
        {
            Dictionary<string, string> headers = new() 
            {
                { "test", "123" },
            };
            Dictionary<string, object> body = new()
            {
                { "test", 123 }
            };

            var mock = new MockHttpMessageHandler();
            mock
                .When("/test")
                .Respond((HttpRequestMessage request) =>
                {
                    request.Method.Should().Be(HttpMethod.Get);

                    var allHeaders = Enumerable.Concat(request.Headers, request.Content.Headers).ToArray();

                    var contentTypeExists = allHeaders.Any(kvp => kvp.Key == "Content-Type");
                    var contentType = allHeaders.FirstOrDefault(kvp => kvp.Key == "Content-Type").Value.FirstOrDefault();
                    contentTypeExists.Should().BeTrue();
                    contentType.Should().Contain("application/json");

                    var testExists = allHeaders.Any(kvp => kvp.Key == "test");
                    var test = allHeaders.FirstOrDefault(kvp => kvp.Key == "test").Value.FirstOrDefault();
                    testExists.Should().BeTrue();
                    test.Should().Be("123");

                    var languageExists = allHeaders.Any(kvp => kvp.Key == "Accept-Language");
                    var language = allHeaders.FirstOrDefault(kvp => kvp.Key == "Accept-Language").Value.FirstOrDefault();
                    languageExists.Should().BeTrue();
                    language.Should().Be("test_lang");

                    return default;
                });


            var httpClient = mock.ToHttpClient();
            var pocketbase = new PocketBase("https://example.com", language: "test_lang", httpClient: httpClient);

            var result = await pocketbase.SendAsync<object>("/test", HttpMethod.Get, headers: headers, body: body);
        }

    }
}