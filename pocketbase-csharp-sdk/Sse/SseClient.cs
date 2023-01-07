using pocketbase_csharp_sdk.Models;
using System.Text;
using System;

namespace pocketbase_csharp_sdk.Sse
{
    public class SseClient
    {
        const string BasePath = "/api/realtime";

        private readonly PocketBase client;
        private CancellationTokenSource? tokenSource = null;
        private Task? eventListenerTask = null;

        public string? Id { get; private set; }
        public bool IsConnected { get; private set; } = false;


        public SseClient(PocketBase client)
        {
            this.client = client;
        }
        ~SseClient()
        {
            Disconnect();
        }

        public async Task ConnectAsync(Action<SseMessage> callback)
        {
            Disconnect();
            tokenSource = new CancellationTokenSource();
            try
            {
                eventListenerTask = ConnectEventStreamAsync(callback, tokenSource.Token);

                while (!IsConnected)
                    await Task.Delay(500);
            }
            catch { throw; }
        }

        public void Disconnect()
        {
            tokenSource?.Cancel();
            tokenSource?.Dispose();
            tokenSource = null;

            eventListenerTask?.Dispose();
            eventListenerTask = null;
        }

        private async Task ConnectEventStreamAsync(Action<SseMessage> callback, CancellationToken token)
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

                var stream = await response.Content.ReadAsStreamAsync(token);
                var buffer = new byte[4096];
                while (!token.IsCancellationRequested)
                {
                    var readCount = await stream.ReadAsync(buffer, token);
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
