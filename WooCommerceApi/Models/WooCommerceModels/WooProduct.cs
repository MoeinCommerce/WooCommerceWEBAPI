using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooCommerceApi.Models.WooCommerceModels
{
    using Newtonsoft.Json;

    public class WooProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Permalink { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public bool Featured { get; set; }
        public string CatalogVisibility { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Sku { get; set; }
        public string Price { get; set; }
        public string RegularPrice { get; set; }
        public string SalePrice { get; set; }
        public bool OnSale { get; set; }
        public string StockStatus { get; set; }
        public bool ManageStock { get; set; }
        public int? StockQuantity { get; set; }  // Nullable to handle null from API
        public bool BackordersAllowed { get; set; }
        public bool SoldIndividually { get; set; }
        public string Weight { get; set; }

        // Nested Dimensions
        public WooProductDimensions Dimensions { get; set; }  // Ensure it's a separate object for handling dimensions

        public List<WooProductCategory> Categories { get; set; } = new List<WooProductCategory>(); // List of categories
        public List<string> Tags { get; set; } = new List<string>(); // List of tags
        public List<WooProductImage> Images { get; set; } = new List<WooProductImage>(); // List of images
        public List<WooProductAttribute> Attributes { get; set; } = new List<WooProductAttribute>(); // List of attributes
        public List<int> Variations { get; set; } = new List<int>(); // List of variations
    }

    public class WooProductDimensions
    {
        public string Length { get; set; } = string.Empty; // Default to empty string
        public string Width { get; set; } = string.Empty;  // Default to empty string
        public string Height { get; set; } = string.Empty; // Default to empty string
    }

    public class WooProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }

    public class WooProductImage
    {
        public int Id { get; set; }
        public string Src { get; set; }
        public string Name { get; set; }
        public string Alt { get; set; }
    }

    public class WooProductAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public bool Visible { get; set; }
        public bool Variation { get; set; }
        public List<string> Options { get; set; } = new List<string>(); // Initialize to avoid null issues
    }

    public class WooProductVariation
    {
        public int Id { get; set; }
        public string Price { get; set; }
        public string RegularPrice { get; set; }
        public string SalePrice { get; set; }
        public bool OnSale { get; set; }
        public bool ManageStock { get; set; }
        public int StockQuantity { get; set; }
        public string StockStatus { get; set; }
        public string Weight { get; set; }
        public WooProductDimensions Dimensions { get; set; } // Nested dimensions
    }

}
