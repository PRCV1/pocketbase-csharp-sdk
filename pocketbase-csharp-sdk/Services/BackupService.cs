using FluentResults;
using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Services
{
    public class BackupService : BaseService
    {
        readonly PocketBase _pocketBase;
        public BackupService(PocketBase pocketBase)
        {
            this._pocketBase = pocketBase;
        }

        protected override string BasePath(string? path = null)
        {
            return path ?? string.Empty;
        }

        public async Task<Result<IEnumerable<BackupModel>>> GetFullListAsync()
        {
            var b = await _pocketBase.SendAsync<IEnumerable<BackupModel>>("/api/backups", HttpMethod.Get);
            return b;
        }

    }
}
