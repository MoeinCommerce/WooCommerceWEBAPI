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
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

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
        
        [JsonProperty("tax_status")]
        public string TaxStatus { get; set; }

        [JsonProperty("tax_class")]
        public string TaxClass { get; set; }

        [JsonProperty("manage_stock")]
        public string ManageStock { get; set; }

        [JsonProperty("stock_quantity")]
        public string StockQuantity { get; set; }

        [JsonProperty("stock_status")]
        public string StockStatus { get; set; }

        [JsonProperty("weight")]
        public string Weight { get; set; }

        [JsonProperty("categories")]
        public List<MinimalWooCategory> Categories { get; set; } // Replaced with WooProductCategory class

        [JsonProperty("attributes")]
        public List<WooAttribute> Attributes { get; set; }
    }
    public class WooAttribute
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("option")]
        public string Option { get; set; }
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

    public class WooProductCategory
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }
    }

    public class WooProductImage
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

    public class WooProductAttribute
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("visible")]
        public bool Visible { get; set; }

        [JsonProperty("variation")]
        public bool Variation { get; set; }

        [JsonProperty("options")]
        public List<string> Options { get; set; } = new List<string>(); // Initialize to avoid null issues
    }
    public class WooProductDownloadModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }
    }

    public class WooProductVariation
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("regular_price")]
        public string RegularPrice { get; set; }

        [JsonProperty("sale_price")]
        public string SalePrice { get; set; }

        [JsonProperty("on_sale")]
        public bool OnSale { get; set; }

        [JsonProperty("manage_stock")]
        public bool ManageStock { get; set; }

        [JsonProperty("stock_quantity")]
        public int StockQuantity { get; set; }

        [JsonProperty("stock_status")]
        public string StockStatus { get; set; }

        [JsonProperty("weight")]
        public string Weight { get; set; }

        [JsonProperty("dimensions")]
        public WooProductDimensions Dimensions { get; set; } // Nested dimensions
    }

}
