using System;
using Newtonsoft.Json;

namespace WooCommerceApi.Models.WooCommerceModels
{
    public class WooCategory : MinimalWooCategory
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("parent")]
        public int? Parent { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class WooCategoryImage
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("src")]
        public string Src { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alt")]
        public string Alt { get; set; }
    }

    public class WooCategoryLinks
    {
        [JsonProperty("self")]
        public WooCategoryLink[] Self { get; set; }

        [JsonProperty("collection")]
        public WooCategoryLink[] Collection { get; set; }
    }

    public class WooCategoryLink
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }
}