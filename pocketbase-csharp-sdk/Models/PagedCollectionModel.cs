namespace pocketbase_csharp_sdk.Models
{
    public class PagedCollectionModel<T>
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalItems { get; set; }
        public T[]? Items { get; set; }
    }

}
