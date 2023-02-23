using pocketbase_csharp_sdk.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
