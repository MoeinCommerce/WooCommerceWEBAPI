using Newtonsoft.Json;

namespace WooCommerceApi.Models.WooCommerceModels
{
    public class MinimalWooCategory
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}