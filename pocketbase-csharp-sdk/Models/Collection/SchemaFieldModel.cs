namespace pocketbase_csharp_sdk.Models.Collection
{
    public class SchemaFieldModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public bool? System { get; set; }
        public bool? Required { get; set; }
        public bool? Unique { get; set; }
        public IDictionary<string, object>? Options { get; set; }
    }
}