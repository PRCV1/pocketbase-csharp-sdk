using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public class CollectionModel
    {
        public string? Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public string? Name { get; set; }
        public bool? System { get; set; }
        public string? ListRule { get; set; }
        public string? ViewRule { get; set; }
        public string? CreateRule { get; set; }
        public string? UpdateRule { get; set; }
        public string? DeleteRule { get; set; }

        public IEnumerable<SchemaFieldModel>? Schema { get; set; }
    }
}
