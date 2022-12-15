using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models.Files
{
    public class FilepathFile : BaseFile, IFile
    {
        public string? FilePath { get; set; }

        public Stream? GetStream()
        {

            if (string.IsNullOrWhiteSpace(FilePath))
            {
                return null;
            }

            try
            {
                return File.OpenRead(FilePath);
            }
            catch
            {
                return null;
            }
        }
    }
}
