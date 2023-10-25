using pocketbase_csharp_sdk.Event;
using System.Collections;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using pocketbase_csharp_sdk.Helper;
using pocketbase_csharp_sdk.Stores;

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


        public BaseAuthStore AuthStore { private set; get; }
        // public AdminService Admin { private set; get; }
        // public UserService User { private set; get; }
        // public LogService Log { private set; get; }
        // public SettingsService Settings { private set; get; }
        // public CollectionService Collections { private set; get; }
        // public RecordService Records { private set; get; }
        // public HealthService Health { private set; get; }

        private readonly string _baseUrl;
        private readonly string _language;
        private readonly HttpClient _httpClient;
        private readonly IUrlBuilder _urlBuilder;

        public PocketBase(string baseUrl, BaseAuthStore? authStore = null, string language = "en-US", HttpClient? httpClient = null)
        {
            this._baseUrl = baseUrl;
            this._language = language;
            this._httpClient = httpClient ?? new HttpClient();

            AuthStore = authStore ?? new AuthStore();
            // Admin = new AdminService(this);
            // User = new UserService(this);
            // Log = new LogService(this);
            // Settings = new SettingsService(this);
            // Collections = new CollectionService(this);
            // Records = new RecordService(this);
            // Health = new HealthService(this);

            _urlBuilder = new UrlBuilder(baseUrl);
        }

        public async Task<T> SendAsync<T>(string path, HttpMethod method,
            IPbQueryParams? queryParams = null, IDictionary<string, string>? headers = null)
        {
            Uri url = _urlBuilder.BuildUrl(path, queryParams);

            HttpRequestMessage requestMessage = CreateRequest(url, method, null, headers);
            
            try
            {
                if (BeforeSend is not null)
                {
                    requestMessage = BeforeSend.Invoke(this, new RequestEventArgs(url, requestMessage));
                }

                var response = await _httpClient.SendAsync(requestMessage);

                if (AfterSend is not null)
                {
                    AfterSend.Invoke(this, new ResponseEventArgs(url, response));
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return default;
        }
        
//         public async Task<Result> SendAsync(string path, HttpMethod method, IDictionary<string, string>? headers = null, IDictionary<string, object?>? query = null, IDictionary<string, object>? body = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default)
//         {
//             headers ??= new Dictionary<string, string>();
//             query ??= new Dictionary<string, object?>();
//             body ??= new Dictionary<string, object>();
//             files ??= new List<IFile>();
//
//             Uri url = _urlBuilder.BuildUrl(path, query);
//
//             HttpRequestMessage request = CreateRequest(url, method, headers: headers, query: query, body: body, files: files);
//
//             try
//             {
//                 if (BeforeSend is not null)
//                 {
//                     request = BeforeSend.Invoke(this, new RequestEventArgs(url, request));
//                 }
//
//                 var response = await _httpClient.SendAsync(request, cancellationToken);
//
//                 if (AfterSend is not null)
//                 {
//                     AfterSend.Invoke(this, new ResponseEventArgs(url, response));
//                 }
//
// #if DEBUG
//                 var json = await response.Content.ReadAsStringAsync(cancellationToken);
// #endif
//
//                 if ((int)response.StatusCode >= 400)
//                 {
//                     var error = new ClientError(method, url.ToString(), (int)response.StatusCode);
//                     return Result.Fail(error);
//                 }
//                 
//                 return Result.Ok();
//             }
//             catch (Exception ex)
//             {
//                 if (ex is HttpRequestException requestException)
//                 {
//                     ClientError error = new ClientError(method, url.ToString(), (int)requestException.StatusCode!);
//                     return Result.Fail(error);
//                 }
//
//                 return Result.Fail(new Error(ex.Message));
//             }
//         }

        // public Result Send(string path, HttpMethod method, IDictionary<string, string>? headers = null, IDictionary<string, object?>? query = null, IDictionary<string, object>? body = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default)
        // {
        //     //RETURN RESULT
        //     
        //     headers ??= new Dictionary<string, string>();
        //     query ??= new Dictionary<string, object?>();
        //     body ??= new Dictionary<string, object>();
        //     files ??= new List<IFile>();
        //
        //     Uri url = _urlBuilder.BuildUrl(path, query);
        //
        //     HttpRequestMessage request = CreateRequest(url, method, headers: headers, query: query, body: body, files: files);
        //
        //     try
        //     {
        //         if (BeforeSend is not null)
        //         {
        //             request = BeforeSend.Invoke(this, new RequestEventArgs(url, request));
        //         }
        //
        //         var response = _httpClient.Send(request, cancellationToken);
        //
        //         if (AfterSend is not null)
        //         {
        //             AfterSend.Invoke(this, new ResponseEventArgs(url, response));
        //         }
        //
        //         if ((int)response.StatusCode >= 400)
        //         {
        //             var error = new ClientError(method, url.ToString(), (int)response.StatusCode);
        //             return Result.Fail(error);
        //         }
        //
        //         return Result.Ok();
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex is HttpRequestException requestException)
        //         {
        //             ClientError error = new ClientError(method, url.ToString(), (int)requestException.StatusCode!);
        //             return Result.Fail(error);
        //         }
        //
        //         return Result.Fail(new Error(ex.Message));
        //     }
        // }
        
//         public async Task<Result<T>> SendAsync<T>(string path, HttpMethod method, IDictionary<string, string>? headers = null, IDictionary<string, object?>? query = null, IDictionary<string, object>? body = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default)
//         {
//             headers ??= new Dictionary<string, string>();
//             query ??= new Dictionary<string, object?>();
//             body ??= new Dictionary<string, object>();
//             files ??= new List<IFile>();
//
//             Uri url = _urlBuilder.BuildUrl(path, query);
//
//             HttpRequestMessage request = CreateRequest(url, method, headers: headers, query: query, body: body, files: files);
//
//             try
//             {
//                 if (BeforeSend is not null)
//                 {
//                     request = BeforeSend.Invoke(this, new RequestEventArgs(url, request));
//                 }
//
//                 var response = await _httpClient.SendAsync(request, cancellationToken);
//
//                 if (AfterSend is not null)
//                 {
//                     AfterSend.Invoke(this, new ResponseEventArgs(url, response));
//                 }
//
// #if DEBUG
//                 var json = await response.Content.ReadAsStringAsync(cancellationToken);
// #endif
//                 
//                 if ((int)response.StatusCode >= 400)
//                 {
//                     ClientError error = new ClientError(method, url.ToString(), (int)response.StatusCode);
//                     return Result.Fail(error);
//                 }
//
//                 var parsedResponse =
//                     await response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions, cancellationToken);
//                 return Result.Ok(parsedResponse!);
//             }
//             catch (Exception ex)
//             {
//                 if (ex is HttpRequestException requestException)
//                 {
//                     ClientError error = new ClientError(method, url.ToString(), (int)requestException.StatusCode!);
//                     return Result.Fail(error);
//                 }
//
//                 return Result.Fail(new Error(ex.Message));
//             }
//         }
        
        // public Result<T> Send<T>(string path, HttpMethod method, IDictionary<string, string>? headers = null, IDictionary<string, object?>? query = null, IDictionary<string, object>? body = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default)
        // {
        //     headers ??= new Dictionary<string, string>();
        //     query ??= new Dictionary<string, object?>();
        //     body ??= new Dictionary<string, object>();
        //     files ??= new List<IFile>();
        //
        //     Uri url = _urlBuilder.BuildUrl(path, query);
        //
        //     HttpRequestMessage request = CreateRequest(url, method, headers: headers, query: query, body: body, files: files);
        //
        //     try
        //     {
        //         if (BeforeSend is not null)
        //         {
        //             request = BeforeSend.Invoke(this, new RequestEventArgs(url, request));
        //         }
        //
        //         var response = _httpClient.Send(request, cancellationToken);
        //
        //         if (AfterSend is not null)
        //         {
        //             AfterSend.Invoke(this, new ResponseEventArgs(url, response));
        //         }
        //
        //         if ((int)response.StatusCode >= 400)
        //         {
        //             ClientError error = new ClientError(method, url.ToString(), (int)response.StatusCode);
        //             return Result.Fail(error);
        //         }
        //
        //         using var stream = response.Content.ReadAsStream();
        //         var parsedResponse = JsonSerializer.Deserialize<T>(stream, jsonSerializerOptions);
        //         return Result.Ok(parsedResponse!);
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex is HttpRequestException requestException)
        //         {
        //             ClientError error = new ClientError(method, url.ToString(), (int)requestException.StatusCode!);
        //             return Result.Fail(error);
        //         }
        //
        //         return Result.Fail(new Error(ex.Message));
        //     }
        // }

        // public async Task<Result<Stream>> GetStreamAsync(string path, IDictionary<string, object?>? query = null, CancellationToken cancellationToken = default)
        // {
        //     query ??= new Dictionary<string, object?>();
        //
        //     Uri url = _urlBuilder.BuildUrl(path, query);
        //
        //     try
        //     {
        //         var stream = await _httpClient.GetStreamAsync(url, cancellationToken);
        //         return Result.Ok(stream);
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex is HttpRequestException requestException)
        //         {
        //             ClientError error = new ClientError(HttpMethod.Get, url.ToString(), (int)requestException.StatusCode!);
        //             return Result.Fail(error);
        //         }
        //
        //         return Result.Fail(new Error(ex.Message));
        //     }
        // }

        private HttpRequestMessage CreateRequest(Uri url, HttpMethod method, IDictionary<string, object>? body = null,  IDictionary<string, string>? headers = null, IEnumerable? files = null)
        {
            HttpRequestMessage request = new HttpRequestMessage();

            if (files is not null) // && files.Any())
            {
                // request = BuildFileRequest(method, url, headers, body, files);
            }
            else
            {
                request = BuildJsonRequest(method, url, headers, body);
            }

            if (headers is null)
            {
                return request;
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

        // private HttpRequestMessage BuildFileRequest(HttpMethod method, Uri url, IDictionary<string, string>? headers, IDictionary<string, object>? body, IEnumerable<IFile> files)
        // {
        //     var request = new HttpRequestMessage(method, url);
        //
        //     if (headers is not null)
        //     {
        //         foreach (var header in headers)
        //         {
        //             request.Headers.Add(header.Key, header.Value);
        //         }
        //     }
        //
        //     var form = new MultipartFormDataContent();
        //     foreach (var file in files)
        //     {
        //         var stream = file.GetStream();
        //         if (stream is null || string.IsNullOrWhiteSpace(file.FieldName) || string.IsNullOrWhiteSpace(file.FileName))
        //         {
        //             continue;
        //         }
        //
        //         var fileContent = new StreamContent(stream);
        //         var mimeType = GetMimeType(file);
        //         fileContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
        //         form.Add(fileContent, file.FieldName, file.FileName);
        //     }
        //
        //
        //     if (body is not null && body.Count > 0)
        //     {
        //         Dictionary<string, string> additionalBody = new Dictionary<string, string>();
        //         foreach (var item in body)
        //         {
        //             if (item.Value is IList valueAsList && item.Value is not string)
        //             {
        //                 for (int i = 0; i < valueAsList.Count; i++)
        //                 {
        //                     var listValue = valueAsList[i]?.ToString();
        //                     if (string.IsNullOrWhiteSpace(listValue))
        //                     {
        //                         continue;
        //                     }
        //                     additionalBody[$"{item.Key}{i}"] = listValue;
        //                 }
        //             }
        //             else
        //             {
        //                 var value = item.Value?.ToString();
        //                 if (string.IsNullOrWhiteSpace(value))
        //                 {
        //                     continue;
        //                 }
        //                 additionalBody[item.Key] = value;
        //             }
        //         }
        //
        //         foreach (var item in additionalBody)
        //         {
        //             var content = new StringContent(item.Value);
        //             form.Add(content, item.Key);
        //         }
        //     }
        //
        //     request.Content = form;
        //     return request;
        // }

    }
}