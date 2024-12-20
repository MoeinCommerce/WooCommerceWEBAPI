using System;
using Newtonsoft.Json;

namespace WooCommerceApi.Models.WooCommerceModels
{
    public class WooCustomer
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        
        [JsonProperty("address_1")]
        public string Address1 { get; set; }
        
        [JsonProperty("address_2")]
        public string Address2 { get; set; }
        
        [JsonProperty("city")]
        public string City { get; set; }
        
        [JsonProperty("state")]
        public string State { get; set; }
        
        [JsonProperty("postcode")]
        public string Postcode { get; set; }
        
        [JsonProperty("country")]
        public string Country { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("phone")]
        public string Phone { get; set; }
        
        [JsonProperty("date_created")]
        public DateTime? DateCreated { get; set; }
    }
}