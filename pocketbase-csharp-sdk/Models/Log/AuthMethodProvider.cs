using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Models.Log
{
    public class AuthMethodProvider
    {
        public string? Name { get; set; }
        public string? State { get; set; }
        public string? CodeVerifier { get; set; }
        public string? CodeChallenge { get; set; }
        public string? CodeChallengeMethod { get; set; }
        public string? AuthUrl { get; set; }
    }
}
