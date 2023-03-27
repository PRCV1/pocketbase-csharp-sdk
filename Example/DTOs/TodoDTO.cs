using pocketbase_csharp_sdk.Models;

namespace Example.DTOs
{
    public class TodoDTO : BaseModel
    {
        public string? Name { get; set; }
        public bool ShowDetails { get; set; } = false;
    }
}
