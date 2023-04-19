﻿using pocketbase_csharp_sdk.Json;
using System.Text.Json.Serialization;

namespace pocketbase_csharp_sdk.Models.Log
{
    public class LogRequestModel
    {
        public string? Id { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Created { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? Updated { get; set; }

        public string? Url { get; set; }
        public string? Method { get; set; }
        public int? Status { get; set; }
        public string? Auth { get; set; }
        public string? RemoteIP { get; set; }
        public string? UserIP { get; set; }
        public string? Referer { get; set; }
        public string? UserAgent { get; set; }
        public IDictionary<string, object>? Meta { get; set; }
    }
}
