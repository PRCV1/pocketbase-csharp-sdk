namespace pocketbase_csharp_sdk.Models.Files
{

    /// <summary>
    /// simple class for uploading files to PocketBase, accepting a Stream
    /// </summary>
    public class StreamFile : BaseFile, IFile
    {
        public Stream? Stream { get; set; }

        public Stream? GetStream()
        {
            return Stream;
        }

        public StreamFile()
        {
            
        }

        public StreamFile(Stream? stream)
        {
            this.Stream = stream;
        }

    }
}
