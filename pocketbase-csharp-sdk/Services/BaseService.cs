using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public abstract class BaseService
    {
        protected abstract string BasePath(string? path = null);

    }
}
