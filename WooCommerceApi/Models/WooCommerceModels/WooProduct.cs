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
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("date_created")]
        public string DateCreated { get; set; }

        [JsonProperty("date_created_gmt")]
        public string DateCreatedGmt { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("featured")]
        public string Featured { get; set; }

        //[JsonProperty("catalog_visibility")]
        //public string CatalogVisibility { get; set; }

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

        [JsonProperty("date_on_sale_from")]
        public string DateOnSaleFrom { get; set; }

        [JsonProperty("date_on_sale_from_gmt")]
        public string DateOnSaleFromGmt { get; set; }

        [JsonProperty("date_on_sale_to")]
        public string DateOnSaleTo { get; set; }

        [JsonProperty("date_on_sale_to_gmt")]
        public string DateOnSaleToGmt { get; set; }

        [JsonProperty("virtual")]
        public string Virtual { get; set; }

        [JsonProperty("downloadable")]
        public bool Downloadable { get; set; }

        [JsonProperty("downloads")]
        public List<string> Downloads { get; set; }

        [JsonProperty("download_limit")]
        public string DownloadLimit { get; set; }

        [JsonProperty("download_expiry")]
        public string DownloadExpiry { get; set; }

        [JsonProperty("external_url")]
        public Uri ExternalUrl { get; set; }

        [JsonProperty("button_text")]
        public string ButtonText { get; set; }

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

        [JsonProperty("backorders")]
        public string Backorders { get; set; }

        [JsonProperty("sold_individually")]
        public string SoldIndividually { get; set; }

        [JsonProperty("weight")]
        public string Weight { get; set; }

        [JsonProperty("dimensions")]
        public WooProductDimensions Dimensions { get; set; } // Replaced with WooProductDimensions class

        [JsonProperty("shipping_class")]
        public string ShippingClass { get; set; }

        [JsonProperty("reviews_allowed")]
        public string ReviewsAllowed { get; set; }

        [JsonProperty("upsell_ids")]
        public List<string> UpsellIds { get; set; }

        [JsonProperty("cross_sell_ids")]
        public List<string> CrossSellIds { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("purchase_note")]
        public string PurchaseNote { get; set; }

        [JsonProperty("categories")]
        public List<WooProductCategory> Categories { get; set; } // Replaced with WooProductCategory class

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("images")]
        public List<WooProductImage> Images { get; set; } // Replaced with WooProductImage class

        [JsonProperty("attributes")]
        public List<WooProductAttribute> Attributes { get; set; } // Replaced with WooProductAttribute class

        [JsonProperty("default_attributes")]
        public List<string> DefaultAttributes { get; set; }

        [JsonProperty("menu_order")]
        public string MenuOrder { get; set; }

        [JsonProperty("meta_data")]
        public List<string> MetaData { get; set; }
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
