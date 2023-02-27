using pocketbase_csharp_sdk.Enum;
using pocketbase_csharp_sdk.Event;
using pocketbase_csharp_sdk.Json;
using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Models.Files;
using pocketbase_csharp_sdk.Services;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;

namespace pocketbase_csharp_sdk
{
    public class PocketBase
    {
        #region Private Fields
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        #endregion

        #region Events
        public delegate HttpRequestMessage BeforeSendEventHandler(object sender, RequestEventArgs e);
        public event BeforeSendEventHandler? BeforeSend;

        public delegate void AfterSendEventHandler(object sender, ResponseEventArgs e);
        public event AfterSendEventHandler? AfterSend;
        #endregion


        public AuthStore AuthStore { private set; get; }
        public AdminService Admin { private set; get; }
        public UserService User { private set; get; }
        public LogService Log { private set; get; }
        public SettingsService Settings { private set; get; }
        public CollectionService Collections { private set; get; }
        public RecordService Records { private set; get; }
        public RealTimeService RealTime { private set; get; }
        public HealthService Health { private set; get; }

        private readonly string _baseUrl;
        private readonly string _language;
        internal readonly HttpClient _httpClient;

        public PocketBase(string baseUrl, AuthStore? authStore = null, string language = "en-US", HttpClient? httpClient = null)
        {
            this._baseUrl = baseUrl;
            this._language = language;
            this._httpClient = httpClient ?? new HttpClient();

            AuthStore = authStore ?? new AuthStore();
            Admin = new AdminService(this);
            User = new UserService(this);
            Log = new LogService(this);
            Settings = new SettingsService(this);
            Collections = new CollectionService(this);
            Records = new RecordService(this);
            RealTime = new RealTimeService(this);
            Health = new HealthService(this);
        }

        public CollectionAuthService<T> AuthCollection<T>(string collectionName)
            where T : IBaseAuthModel
        {
            return new CollectionAuthService<T>(this, collectionName);
        }

        public async Task SendAsync(string path, HttpMethod method, IDictionary<string, string>? headers = null, IDictionary<string, object?>? query = null, IDictionary<string, object>? body = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default)
        {
            headers ??= new Dictionary<string, string>();
            query ??= new Dictionary<string, object?>();
            body ??= new Dictionary<string, object>();
            files ??= new List<IFile>();

            Uri url = BuildUrl(path, query);

            HttpRequestMessage request = CreateRequest(url, method, headers: headers, query: query, body: body, files: files);

            try
            {
                if (BeforeSend is not null)
                {
                    request = BeforeSend.Invoke(this, new RequestEventArgs(url, request));
                }

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (AfterSend is not null)
                {
                    AfterSend.Invoke(this, new ResponseEventArgs(url, response));
                }

#if DEBUG
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

                if ((int)response.StatusCode >= 400)
                {
                    //TODO
                    //var dic = GetResponseAsDictionary(responseData);
                    //throw new ClientException(url.ToString(), statusCode: (int)response.StatusCode, response: dic);
                    throw new ClientException(url.ToString(), statusCode: (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                if (ex is ClientException)
                {
                    throw;
                }
                else if (ex is HttpRequestException)
                {
                    throw new ClientException(url: url.ToString(), originalError: ex, isAbort: true);
                }
                else
                {
                    throw new ClientException(url: url.ToString(), originalError: ex);
                }
            }
        }

        public void Send(string path, HttpMethod method, IDictionary<string, string>? headers = null, IDictionary<string, object?>? query = null, IDictionary<string, object>? body = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default)
        {
            headers ??= new Dictionary<string, string>();
            query ??= new Dictionary<string, object?>();
            body ??= new Dictionary<string, object>();
            files ??= new List<IFile>();

            Uri url = BuildUrl(path, query);

            HttpRequestMessage request = CreateRequest(url, method, headers: headers, query: query, body: body, files: files);

            try
            {
                if (BeforeSend is not null)
                {
                    request = BeforeSend.Invoke(this, new RequestEventArgs(url, request));
                }

                var response = _httpClient.Send(request, cancellationToken);

                if (AfterSend is not null)
                {
                    AfterSend.Invoke(this, new ResponseEventArgs(url, response));
                }

                if ((int)response.StatusCode >= 400)
                {
                    //TODO
                    //var dic = GetResponseAsDictionary(responseData);
                    //throw new ClientException(url.ToString(), statusCode: (int)response.StatusCode, response: dic);
                    throw new ClientException(url.ToString(), statusCode: (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                if (ex is ClientException)
                {
                    throw;
                }
                else if (ex is HttpRequestException)
                {
                    throw new ClientException(url: url.ToString(), originalError: ex, isAbort: true);
                }
                else
                {
                    throw new ClientException(url: url.ToString(), originalError: ex);
                }
            }
        }

        public async Task<T?> SendAsync<T>(string path, HttpMethod method, IDictionary<string, string>? headers = null, IDictionary<string, object?>? query = null, IDictionary<string, object>? body = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default)
        {
            headers ??= new Dictionary<string, string>();
            query ??= new Dictionary<string, object?>();
            body ??= new Dictionary<string, object>();
            files ??= new List<IFile>();

            Uri url = BuildUrl(path, query);

            HttpRequestMessage request = CreateRequest(url, method, headers: headers, query: query, body: body, files: files);

            try
            {
                if (BeforeSend is not null)
                {
                    request = BeforeSend.Invoke(this, new RequestEventArgs(url, request));
                }

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (AfterSend is not null)
                {
                    AfterSend.Invoke(this, new ResponseEventArgs(url, response));
                }

#if DEBUG
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

                if ((int)response.StatusCode >= 400)
                {
                    //TODO
                    //var dic = GetResponseAsDictionary(responseData);
                    //throw new ClientException(url.ToString(), statusCode: (int)response.StatusCode, response: dic);
                    throw new ClientException(url.ToString(), statusCode: (int)response.StatusCode);
                }

                return await response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions, cancellationToken);
            }
            catch (Exception ex)
            {
                if (ex is ClientException)
                {
                    throw;
                }
                else if (ex is HttpRequestException)
                {
                    throw new ClientException(url: url.ToString(), originalError: ex, isAbort: true);
                }
                else
                {
                    throw new ClientException(url: url.ToString(), originalError: ex);
                }
            }
        }
        public T? Send<T>(string path, HttpMethod method, IDictionary<string, string>? headers = null, IDictionary<string, object?>? query = null, IDictionary<string, object>? body = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default)
        {
            headers ??= new Dictionary<string, string>();
            query ??= new Dictionary<string, object?>();
            body ??= new Dictionary<string, object>();
            files ??= new List<IFile>();

            Uri url = BuildUrl(path, query);

            HttpRequestMessage request = CreateRequest(url, method, headers: headers, query: query, body: body, files: files);

            try
            {
                if (BeforeSend is not null)
                {
                    request = BeforeSend.Invoke(this, new RequestEventArgs(url, request));
                }

                var response = _httpClient.Send(request, cancellationToken);

                if (AfterSend is not null)
                {
                    AfterSend.Invoke(this, new ResponseEventArgs(url, response));
                }

                if ((int)response.StatusCode >= 400)
                {
                    //TODO
                    //var dic = GetResponseAsDictionary(responseData);
                    //throw new ClientException(url.ToString(), statusCode: (int)response.StatusCode, response: dic);
                    throw new ClientException(url.ToString(), statusCode: (int)response.StatusCode);
                }
                using (var stream = response.Content.ReadAsStream())
                    return JsonSerializer.Deserialize<T>(stream, jsonSerializerOptions);
            }
            catch (Exception ex)
            {
                if (ex is ClientException)
                {
                    throw;
                }
                else if (ex is HttpRequestException)
                {
                    throw new ClientException(url: url.ToString(), originalError: ex, isAbort: true);
                }
                else
                {
                    throw new ClientException(url: url.ToString(), originalError: ex);
                }
            }
        }

        public Task<Stream> GetStreamAsync(string path, IDictionary<string, object?>? query = null, CancellationToken cancellationToken = default)
        {
            query ??= new Dictionary<string, object?>();

            Uri url = BuildUrl(path, query);

            try
            {
                return _httpClient.GetStreamAsync(url, cancellationToken);
            }
            catch (Exception ex)
            {
                if (ex is ClientException)
                {
                    throw;
                }
                else if (ex is HttpRequestException)
                {
                    throw new ClientException(url: url.ToString(), originalError: ex, isAbort: true);
                }
                else
                {
                    throw new ClientException(url: url.ToString(), originalError: ex);
                }
            }
        }

        private HttpRequestMessage CreateRequest(Uri url, HttpMethod method, IDictionary<string, string> headers, IDictionary<string, object?> query, IDictionary<string, object> body, IEnumerable<IFile> files)
        {
            HttpRequestMessage request;

            if (files.Count() > 0)
            {
                request = BuildFileRequest(method, url, headers, body, files);
            }
            else
            {
                request = BuildJsonRequest(method, url, headers, body);
            }

            if (!headers.ContainsKey("Authorization") && AuthStore.IsValid)
            {
                request.Headers.Add("Authorization", AuthStore.Token);
            }

            if (!headers.ContainsKey("Accept-Language"))
            {
                request.Headers.Add("Accept-Language", _language);
            }

            return request;
        }

        private IDictionary<string, object?>? GetResponseAsDictionary(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                return null;
            }
            var dic = new Dictionary<string, object?>();
            var obj = JsonDocument.Parse(response);
            foreach (var item in obj.RootElement.EnumerateObject())
            {

            }
            return dic;
        }

        public Uri BuildUrl(string path, IDictionary<string, object?>? queryParameters = null)
        {
            var url = _baseUrl + (_baseUrl.EndsWith("/") ? "" : "/");

            if (!string.IsNullOrWhiteSpace(path))
            {
                url += path.StartsWith("/") ? path.Substring(1) : path;
            }

            if (queryParameters is not null)
            {
                var query = NormalizeQueryParameters(queryParameters);

                List<string> urlSegments = new();
                foreach (var kvp in query)
                {
                    var encodedKey = HttpUtility.UrlEncode(kvp.Key);
                    foreach (var item in kvp.Value)
                    {
                        var encodedValue = HttpUtility.UrlEncode(item.ToString());
                        urlSegments.Add($"{encodedKey}={encodedValue}");
                    }
                }

                var queryString = string.Join("&", urlSegments);

                if (!string.IsNullOrWhiteSpace(queryString))
                {
                    url = url + "?" + queryString;
                }
            }

            return new Uri(url, UriKind.RelativeOrAbsolute);
        }

        private IDictionary<string, IEnumerable> NormalizeQueryParameters(IDictionary<string, object?>? parameters)
        {
            Dictionary<string, IEnumerable> result = new();

            if (parameters is null)
            {
                return result;
            }

            foreach (var item in parameters)
            {
                List<string> normalizedValue = new();
                IEnumerable valueAsList;

                if (item.Value is IEnumerable && item.Value is not string)
                {
                    valueAsList = (IEnumerable)item.Value;
                }
                else
                {
                    valueAsList = new List<object?>() { item.Value };
                }

                foreach (var subItem in valueAsList)
                {
                    if (subItem is null)
                    {
                        continue;
                    }
                    normalizedValue.Add(subItem.ToString() ?? "");
                }

                if (normalizedValue.Count > 0)
                {
                    result[item.Key] = normalizedValue;
                }
            }

            return result;
        }

        private HttpRequestMessage BuildJsonRequest(HttpMethod method, Uri url, IDictionary<string, string>? headers = null, IDictionary<string, object>? body = null)
        {
            var request = new HttpRequestMessage(method, url);
            if (body is not null && body.Count > 0)
            {
                request.Content = JsonContent.Create(body);
            }

            if (headers is not null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            return request;
        }

        private HttpRequestMessage BuildFileRequest(HttpMethod method, Uri url, IDictionary<string, string>? headers, IDictionary<string, object>? body, IEnumerable<IFile> files)
        {
            var request = new HttpRequestMessage(method, url);

            if (headers is not null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var form = new MultipartFormDataContent();
            foreach (var file in files)
            {
                var stream = file.GetStream();
                if (stream is null || string.IsNullOrWhiteSpace(file.FieldName) || string.IsNullOrWhiteSpace(file.FileName))
                {
                    continue;
                }

                var fileContent = new StreamContent(stream);
                var mimeType = GetMimeType(file);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                form.Add(fileContent, file.FieldName, file.FileName);
            }


            if (body is not null && body.Count > 0)
            {
                Dictionary<string, string> additionalBody = new Dictionary<string, string>();
                foreach (var item in body)
                {
                    if (item.Value is IList valueAsList && item.Value is not string)
                    {
                        for (int i = 0; i < valueAsList.Count; i++)
                        {
                            var listValue = valueAsList[i]?.ToString();
                            if (string.IsNullOrWhiteSpace(listValue))
                            {
                                continue;
                            }
                            additionalBody[$"{item.Key}{i}"] = listValue;
                        }
                    }
                    else
                    {
                        var value = item.Value?.ToString();
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            continue;
                        }
                        additionalBody[item.Key] = value;
                    }
                }

                foreach (var item in additionalBody)
                {
                    var content = new StringContent(item.Value);
                    form.Add(content, item.Key);
                }
            }

            request.Content = form;
            return request;
        }

        private string GetMimeType(IFile file)
        {
            if (file is FilepathFile filePath)
            {
                var fileName = Path.GetFileName(filePath.FilePath);
                return MimeMapping.MimeUtility.GetMimeMapping(fileName);
            }
            return MimeMapping.MimeUtility.UnknownMimeType;
        }

    }
}