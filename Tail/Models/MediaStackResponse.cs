using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace Tail.Models
{
    public class MediaStackResponse : BaseModel
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("data")]
        public List<Data> ListData { get; set; }
    }

    public class Data : BaseModel
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("image")]
        public Uri Image { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("published_at")]
        public DateTimeOffset PublishedAt { get; set; }
    }

    public class Pagination : BaseModel
    {
        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }


}
