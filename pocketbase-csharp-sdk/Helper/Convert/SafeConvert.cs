namespace pocketbase_csharp_sdk.Helper.Convert
{
    public static class SafeConvert
    {
        public static int ToInt(this object? obj, int defaultValue = 0)
        {
            if (obj is null)
            {
                return defaultValue;
            }

            if (int.TryParse(obj.ToString(), out var result))
            {
                return result;
            }

            return defaultValue;
        }

        public static string ToString(this object? obj, string defaultValue = "")
        {
            if (obj is null)
            {
                return defaultValue;
            }

            return obj.ToString()!;
        }

    }
}
