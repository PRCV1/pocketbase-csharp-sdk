using pocketbase_csharp_sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk
{
    public class AuthStoreEvent
    {

        public string? Token { get; private set; }
        public BaseModel? Model { get; private set; }

        public AuthStoreEvent(string? token, BaseModel? model)
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
