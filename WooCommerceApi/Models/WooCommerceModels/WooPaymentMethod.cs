
using Newtonsoft.Json;

namespace WooCommerceApi.Models.WooCommerceModels
{
    public class WooPaymentMethod
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}