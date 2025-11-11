using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooCommerceApi.Models.WooCommerceModels
{
    public class WooProduct
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date_created")]
        public DateTime? DateCreated { get; set; }
        
        [JsonProperty("date_modified")]
        public DateTime? DateModified { get; set; }

        [JsonProperty("date_created_gmt")]
        public DateTime? DateCreatedGmt { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("short_description")]
        public string ShortDescription { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("regular_price")]
        public string RegularPrice { get; set; }

        [JsonProperty("sale_price")]
        public string SalePrice { get; set; }

        [JsonProperty("manage_stock")]
        public string ManageStock { get; set; }

        [JsonProperty("stock_quantity")]
        public string StockQuantity { get; set; }

        [JsonProperty("stock_status")]
        public string StockStatus { get; set; }

        [JsonProperty("categories")]
        public List<MinimalWooCategory> Categories { get; set; } // Replaced with WooProductCategory class

        [JsonProperty("attributes")]
        public List<WooAttribute> Attributes { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
    public class WooAttribute
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("variation")]
        public bool Variation { get; set; } = true;

        [JsonProperty("visible")]
        public bool Visible { get; set; } = true;

        [JsonProperty("option")]
        public string Option { get; set; }

        [JsonProperty("options")]
        public List<string> Options { get; set; }

        public List<WooAttributeTerm> Terms { get; set; } = new List<WooAttributeTerm>();
    }
    public class WooAttributeTerm
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }


    public class WooProductDimensions
    {
        [JsonProperty("length")]
        public string Length { get; set; } = string.Empty;

        [JsonProperty("width")]
        public string Width { get; set; } = string.Empty;

        [JsonProperty("height")]
        public string Height { get; set; } = string.Empty;
    }
    public class WooProductVariation
    {

        //[JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("date_created")]
        public DateTime? DateCreated { get; set; }

        [JsonProperty("date_modified")]
        public DateTime? DateModified { get; set; }

        [JsonProperty("date_created_gmt")]
        public DateTime? DateCreatedGmt { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("regular_price")]
        public string RegularPrice { get; set; }

        [JsonProperty("sale_price")]
        public string SalePrice { get; set; }

        [JsonProperty("manage_stock")]
        public string ManageStock { get; set; }

        [JsonProperty("stock_quantity")]
        public string StockQuantity { get; set; }

        [JsonProperty("attributes")]
        public List<WooAttribute> Attributes { get; set; }
    }

}
