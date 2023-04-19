namespace pocketbase_csharp_sdk.Models.Collection
{
    public class CollectionOptionsModel
    {
        public bool? AllowEmailAuth { get; set; }
        public bool? AllowOAuth2Auth { get; set; }
        public bool? AllowUsernameAuth { get; set; }
        public bool? RequireEmail { get; set; }
        public int? MinPasswordLength { get; set; }

        public string? ExceptEmailDomains { get; set; }
        public string? OnlyEmailDomains { get; set; }

        public string? ManageRule { get; set; }
    }
}
