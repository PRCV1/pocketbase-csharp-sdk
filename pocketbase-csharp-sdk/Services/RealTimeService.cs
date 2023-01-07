using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Sse;

namespace pocketbase_csharp_sdk.Services
{
    public class RealTimeService : BaseService
    {
        protected override string BasePath(string? path = null) => "/api/realtime";

        private readonly PocketBase client;

        private SseClient? _SseClient = null;
        private SseClient SseClient => _SseClient ??= new SseClient(client, RealTimeCallBack);

        private Dictionary<string, List<Action<SseMessage>>> Subscriptions = new Dictionary<string, List<Action<SseMessage>>>();

        public RealTimeService(PocketBase client)
        {
            this.client = client;
        }

        private void RealTimeCallBack(SseMessage message)
        {
            var messageEvent = message.Event ?? "";
            if (Subscriptions.ContainsKey(messageEvent))
                foreach (var callBack in Subscriptions[messageEvent])
                    callBack(message);
        }

        public async Task SubscribeAsync(string subscription, Action<SseMessage> callback)
        {
            if (!Subscriptions.ContainsKey(subscription))
            {
                // New subscription
                Subscriptions.Add(subscription, new List<Action<SseMessage>> { callback });
                await SubmitSubscriptionsAsync();
            }
            else
            {
                var subcriptionCallbacks = Subscriptions[subscription];
                if (!subcriptionCallbacks.Contains(callback))
                    subcriptionCallbacks.Add(callback);
            }
        }
        public async Task UnsubscribeAsync(string? topic = null)
        {
            if (string.IsNullOrEmpty(topic))
                Subscriptions.Clear();
            else if (Subscriptions.ContainsKey(topic))
                Subscriptions.Remove(topic);
            else
                return;
            await SubmitSubscriptionsAsync();
        }
        public async Task UnsubscribeByPrefix(string prefix)
        {
            var subscriptionsToRemove = Subscriptions.Keys.Where(k => k.StartsWith(prefix)).ToList();
            if (subscriptionsToRemove.Any())
            {
                foreach (var subs in subscriptionsToRemove)
                    Subscriptions.Remove(subs);

                await SubmitSubscriptionsAsync();
            }
        }
        public async Task UnsubscribeByTopicAndListener(string topic, Action<SseMessage> listener)
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
