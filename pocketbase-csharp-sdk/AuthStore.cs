using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk
{
    public class AuthStore
    {

        public event EventHandler<AuthStoreEvent>? OnChange;

        public string? Token { get; set; }
        public BaseModel? Model { get; set; }

        public bool IsValid { get => GetIsValid(); }

        private bool GetIsValid()
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                return false;
            }

            var parts = Token.Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                return false;
            }

            string payload = Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]));
            var encoded = JsonSerializer.Deserialize<IDictionary<string, object>>(payload)!;

            if (encoded["exp"] is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Number)
            {
                var exp = jsonElement.GetInt32();
                var expiredAt = DateTimeOffset.FromUnixTimeSeconds(exp);
                return expiredAt > DateTimeOffset.Now;
            }

            return false;
        }

        public void Save(string? token, BaseModel? model)
        {
            this.Token = token;
            this.Model = model;

            OnChange?.Invoke(this, new AuthStoreEvent(this.Token, this.Model));
        }

        public void Clear()
        {
            this.Token = null;
            this.Model = null;

            OnChange?.Invoke(this, new AuthStoreEvent(this.Token, this.Model));
        }
    }
}
