using pocketbase_csharp_sdk.Models;

namespace Example.Models
{
    public class EntryModel : BaseModel
    {
        public string? Name { get; set; }
        public bool IsDone { get; set; }
        public string? Todo_Id { get; set; }
    }
}
