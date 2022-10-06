using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public abstract class BaseModel
    {
        public string? Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public string? Email { get; set; }
        public DateTime? LastResetSentAt { get; set; }
    }
}
