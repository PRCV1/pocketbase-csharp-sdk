using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Services.Base;
using pocketbase_csharp_sdk.Sse;

namespace pocketbase_csharp_sdk.Services
{
    public class RealTimeService : BaseService
    {
        protected override string BasePath(string? path = null) => "/api/realtime";

        private readonly PocketBase _client;

        private SseClient? _sseClient = null;
        private SseClient SseClient => _sseClient ??= new SseClient(_client, RealTimeCallBackAsync);

        private readonly Dictionary<string, List<Func<SseMessage, Task>>> _subscriptions = new();

        public RealTimeService(PocketBase client)
        {
            this._client = client;
        }

        private async Task RealTimeCallBackAsync(SseMessage message)
        {
            var messageEvent = message.Event ?? "";
            if (_subscriptions.ContainsKey(messageEvent))
                foreach (var callBack in _subscriptions[messageEvent])
                    await callBack(message);
        }

        public async Task SubscribeAsync(string subscription, Func<SseMessage, Task> callback)
        {
            if (!_subscriptions.ContainsKey(subscription))
            {
                // New subscription
                _subscriptions.Add(subscription, new List<Func<SseMessage, Task>> { callback });
                await SubmitSubscriptionsAsync();
            }
            else
            {
                var subcriptionCallbacks = _subscriptions[subscription];
                if (!subcriptionCallbacks.Contains(callback))
                    subcriptionCallbacks.Add(callback);
            }
        }

        public Task UnsubscribeAsync(string? topic = null)
        {
            if (string.IsNullOrEmpty(topic))
                _subscriptions.Clear();
            else if (_subscriptions.ContainsKey(topic))
                _subscriptions.Remove(topic);
            else
                return Task.CompletedTask;
            return SubmitSubscriptionsAsync();
        }

        public async Task UnsubscribeByPrefixAsync(string prefix)
        {
            var subscriptionsToRemove = _subscriptions.Keys.Where(k => k.StartsWith(prefix)).ToList();
            if (subscriptionsToRemove.Any())
            {
                foreach (var subs in subscriptionsToRemove)
                    _subscriptions.Remove(subs);

                await SubmitSubscriptionsAsync();
            }
        }

        public async Task UnsubscribeByTopicAndListenerAsync(string topic, Func<SseMessage, Task> listener)
        {
            if (!_subscriptions.ContainsKey(topic))
                return;

            var listeners = _subscriptions[topic];
            if (listeners.Remove(listener) && !listeners.Any())
                await UnsubscribeAsync(topic);
        }

        private async Task SubmitSubscriptionsAsync()
        {
            if (!_subscriptions.Any())
                SseClient.Disconnect();
            else
            {
                await SseClient.EnsureIsConnectedAsync();
                Dictionary<string, object> body = new()
                {
                    { "clientId", SseClient.Id! },
                    { "subscriptions", _subscriptions.Keys.ToList() }
                };

                await _client.SendAsync(BasePath(), HttpMethod.Post, body: body);
            }
        }
    }
}
