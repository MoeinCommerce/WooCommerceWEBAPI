using Newtonsoft.Json;

namespace WooCommerceApi.Models.WooCommerceModels
{
    public class MinimalWooCategory
    {
        [JsonProperty("id")]
        public long Id { get; set; }
    }
}