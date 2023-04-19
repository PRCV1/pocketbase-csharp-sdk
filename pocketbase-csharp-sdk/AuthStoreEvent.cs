using pocketbase_csharp_sdk.Models;

namespace pocketbase_csharp_sdk
{
    public class AuthStoreEvent
    {

        public string? Token { get; private set; }
        public IBaseModel? Model { get; private set; }

        public AuthStoreEvent(string? token, IBaseModel? model)
        {
            Token = token;
            Model = model;
        }

        public override string ToString()
        {
            return $"token: {Token}{Environment.NewLine}model: {Model}";
        }

    }
}
