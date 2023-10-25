using System.Text;
using System.Text.Json;

namespace pocketbase_csharp_sdk.Stores
{
    public abstract class BaseAuthStore
    {
        public string? Token { get; set; }
        public object? AuthModel { get; set; }
        public bool IsValid {
            get => IsTokenValid(Token);
        }

        public delegate void OnSaveEventHandler(object sender, BaseAuthStore authStore);

        public event OnSaveEventHandler? OnSave;

        public void Save(string token, object authModel)
        {
            this.Token = token;
            this.AuthModel = authModel;

            OnSave?.Invoke(this, this);
        }

        public void Clear()
        {
            this.Token = null;
            this.AuthModel = null;

            OnSave?.Invoke(this, this);
        }

        protected bool IsTokenValid(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            var parts = token.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                return false;
            }

            string rawPayload = parts[1];
            string payload = Encoding.UTF8.GetString(ParseJwtPayload(rawPayload));
            var encodedPayload = JsonSerializer.Deserialize<IDictionary<string, object>>(payload);

            if (encodedPayload is null)
            {
                return false;
            }

            if (encodedPayload["exp"] is JsonElement { ValueKind: JsonValueKind.Number } jsonElement)
            {
                var exp = jsonElement.GetInt32();
                var expireAt = DateTimeOffset.FromUnixTimeSeconds(exp);
                return expireAt > DateTimeOffset.Now;
            }

            return false;
        }

        private byte[] ParseJwtPayload(string payload)
        {
            switch (payload.Length % 4)
            {
                case 2:
                    payload += "==";
                    break;
                case 3:
                    payload += "=";
                    break;
            }

            return Convert.FromBase64String(payload);
        }
    }
}