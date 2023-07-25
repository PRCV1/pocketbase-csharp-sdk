﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace pocketbase_csharp_sdk.Json
{
    public class DateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (!DateTime.TryParse(value, out var dt))
            {
                return null;
            }

            return DateTime.SpecifyKind(dt, DateTimeKind.Utc); ;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value?.ToString());
            }
        }
    }
}
