using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pocketbase_csharp_sdk.Helper.Extensions
{
    internal static class DictionaryExtensions
    {
        public static void AddIfNotNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue? value)
        {
            if (value is null)
            {
                return;
            }
            dictionary[key] = value;
        }
    }
}
