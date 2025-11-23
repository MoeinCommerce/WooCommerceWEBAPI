using Newtonsoft.Json;

namespace WooCommerceApi.Models.WooCommerceModels
{
    public class MinimalWooCategory
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}