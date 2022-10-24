using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models
{
    public class ResultList<T> where T : class
    {
        public int? Page { get; set; }
        public int? PerPage { get; set; }
        public int? TotalItems { get; set; }
        public int? TotalPages { get; set; }
        public IEnumerable<T>? Items { get; set; }
    }
}
