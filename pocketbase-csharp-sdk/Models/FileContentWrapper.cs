using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public class FileContentWrapper
    {
        public Stream? Stream { get; set; }
        public string? FileName { get; set; }
    }
}
