using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models.Files
{
    public interface IFile
    {
        public string? FieldName { get; set; }

        public Stream? GetStream();

    }
}
