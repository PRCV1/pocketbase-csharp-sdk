namespace pocketbase_csharp_sdk.Models
{
    public interface IBaseModel
    {
        string? Id { get; }

        DateTime? Created { get; }

        DateTime? Updated { get; }

        string? CollectionId { get; }

        string? CollectionName { get; }
    }
}
