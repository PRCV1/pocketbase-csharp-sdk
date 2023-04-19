using pocketbase_csharp_sdk.Json;
using System.Text.Json.Serialization;

namespace pocketbase_csharp_sdk.Models.Log
{
    public class LogRequestStatistic
    {
        public int? Total { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Date { get; set; }
    }
}
