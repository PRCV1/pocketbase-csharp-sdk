using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models.Files
{

    /// <summary>
    /// simple class for uploading files to PocketBase, accepting a path to a file
    /// </summary>
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

        public FilepathFile()
        {
            
        }

        public FilepathFile(string? filePath)
        {
            this.FilePath = filePath;
        }

    }
}
