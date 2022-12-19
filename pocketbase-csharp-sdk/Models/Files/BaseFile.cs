using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models.Files
{
    public abstract class BaseFile
    {
        public string? FieldName { get; set; }
        public string? FileName { get; set; }
    }
}
