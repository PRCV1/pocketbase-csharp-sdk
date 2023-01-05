using pocketbase_csharp_sdk.Models;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace pocketbase_csharp_sdk.Sse
{
    public class SseClient
    {
        const string BasePath = "/api/realtime";
        public string Id { get; private set; }

        private readonly PocketBase client;
        private CancellationTokenSource? tokenSource = null;
        private Task? eventListenerTask = null;

        int _retryAttempts = 0;
        int _maxRetry = 5;
        Timer? _retryTimer;

        /// Indicates whether the client was closed.
        public bool IsClosed { get; private set; } = false;
        public bool IsConnected { get; private set; } = false;

        public SseClient(PocketBase client)
        {
            this.client = client;
        }


        public async Task ConnectAsync(Action<SseMessage> callback)
        {
            Disconnect();
            tokenSource = new CancellationTokenSource();
            try
            {
                eventListenerTask = ConnectEventStreamAsync(tokenSource.Token, callback);

                while (!IsConnected)
                    await Task.Delay(500);
            }
            catch { throw; }

        }

        public void Disconnect()
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
            tokenSource = null;

            if(eventListenerTask!= null)
                eventListenerTask.Dispose();
            eventListenerTask = null;
        }

        private async Task ConnectEventStreamAsync(CancellationToken token, Action<SseMessage> callback)
        {
            var httpClient = new HttpClient
            {
                Timeout = Timeout.InfiniteTimeSpan
            };
            try
            {
                var response = await httpClient.GetAsync(client.BuildUrl(BasePath).ToString(),
                                                         HttpCompletionOption.ResponseHeadersRead,
                                                         token);
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Unable to connect the stream");

                var isTextEventStream = response.Content.Headers.ContentType?.MediaType == "text/event-stream";

                if (!isTextEventStream)
                    throw new InvalidOperationException("Invalid resource content type");

                var stream = await response.Content.ReadAsStreamAsync();
                var buffer = new byte[4096];
                while (!token.IsCancellationRequested)
                {
                    var readCount = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                    if (readCount > 0)
                    {
                        var data = Encoding.UTF8.GetString(buffer, 0, readCount);
                        var sseMessage = await SseMessage.FromReceivedMessage(data);
                        if (sseMessage != null)
                        {
                            if (sseMessage.Id != null && sseMessage.Event == "PB_CONNECT")
                            {
                                Id = sseMessage.Id;
                                IsConnected = true;
                            }
                            callback(sseMessage);
                        }
                    }
                    await Task.Delay(125, token);
                }
            }
            finally
            {
                httpClient.Dispose();
                IsConnected = false;
            }
        }
    }
}
