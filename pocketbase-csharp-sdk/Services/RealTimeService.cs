using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Sse;

namespace pocketbase_csharp_sdk.Services
{
    public class RealTimeService : BaseService
    {
        protected override string BasePath(string? path = null) => "/api/realtime";

        private readonly PocketBase client;

        private SseClient? _SseClient = null;
        private SseClient SseClient => _SseClient ??= new SseClient(client, RealTimeCallBackAsync);

        private readonly Dictionary<string, List<Func<SseMessage, Task>>> Subscriptions = new();

        public RealTimeService(PocketBase client)
        {
            this.client = client;
        }

        private async Task RealTimeCallBackAsync(SseMessage message)
        {
            var messageEvent = message.Event ?? "";
            if (Subscriptions.ContainsKey(messageEvent))
                foreach (var callBack in Subscriptions[messageEvent])
                    await callBack(message);
        }

        public async Task SubscribeAsync(string subscription, Func<SseMessage, Task> callback)
        {
            if (!Subscriptions.ContainsKey(subscription))
            {
                // New subscription
                Subscriptions.Add(subscription, new List<Func<SseMessage, Task>> { callback });
                await SubmitSubscriptionsAsync();
            }
            else
            {
                var subcriptionCallbacks = Subscriptions[subscription];
                if (!subcriptionCallbacks.Contains(callback))
                    subcriptionCallbacks.Add(callback);
            }
        }

        public Task UnsubscribeAsync(string? topic = null)
        {
            if (string.IsNullOrEmpty(topic))
                Subscriptions.Clear();
            else if (Subscriptions.ContainsKey(topic))
                Subscriptions.Remove(topic);
            else
                return Task.CompletedTask;
            return SubmitSubscriptionsAsync();
        }

        public async Task UnsubscribeByPrefixAsync(string prefix)
        {
            var subscriptionsToRemove = Subscriptions.Keys.Where(k => k.StartsWith(prefix)).ToList();
            if (subscriptionsToRemove.Any())
            {
                foreach (var subs in subscriptionsToRemove)
                    Subscriptions.Remove(subs);

                await SubmitSubscriptionsAsync();
            }
        }

        public async Task UnsubscribeByTopicAndListenerAsync(string topic, Func<SseMessage, Task> listener)
        {
            if (!Subscriptions.ContainsKey(topic))
                return;

            var listeners = Subscriptions[topic];
            if (listeners.Remove(listener) && !listeners.Any())
                await UnsubscribeAsync(topic);
        }

        private async Task SubmitSubscriptionsAsync()
        {
            if (!Subscriptions.Any())
                SseClient.Disconnect();
            else
            {
                await SseClient.EnsureIsConnectedAsync();
                Dictionary<string, object> body = new()
                {
                    { "clientId", SseClient.Id! },
                    { "subscriptions", Subscriptions.Keys.ToList() }
                };

                await client.SendAsync(BasePath(), HttpMethod.Post, body: body);
            }
        }
    }
}
