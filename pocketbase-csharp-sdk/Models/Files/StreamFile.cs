using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models.Files
{
    public class StreamFile : BaseFile, IFile
    {
        public Stream? Stream { get; set; }

        public Stream? GetStream()
        {
            return Stream;
        }
    }
}
