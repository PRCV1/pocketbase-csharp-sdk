namespace pocketbase_csharp_sdk.Helper
{
    public class PbListQueryParams : IPbQueryParams
    {
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 30;
        public string Sort { get; set; } = string.Empty;
        public string Filter { get; set; } = string.Empty;
        
        public IDictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>() {
                { "page", Page.ToString() },
                { "perPage", PerPage.ToString() },
                { "sort", Sort },
                { "filter", Filter },
            };
        }
    }
}