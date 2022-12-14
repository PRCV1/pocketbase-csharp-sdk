using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public class SseMessage
    {
        public string? Id { get; set; }
        public string? Event { get; set; }
        public string? Data { get; set; }
        public int? Retry { get; set; }
    }
}
