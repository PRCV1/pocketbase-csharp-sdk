namespace pocketbase_csharp_sdk.Models.Files
{

    /// <summary>
    /// simple Interface needed for uploading files to PocketBase
    /// </summary>
    public interface IFile
    {
        public string? FieldName { get; set; }
        public string? FileName { get; set; }
        public Stream? GetStream();

    }
}
