using System;
using Newtonsoft.Json;

namespace WooCommerceApi.Models.WooCommerceModels
{
    public class WooCategory : MinimalWooCategory
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent")]
        public long? Parent { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class WooCategoryLink
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }
}