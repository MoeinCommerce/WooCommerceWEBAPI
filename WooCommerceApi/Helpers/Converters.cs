using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WooCommerceApi.Models.WooCommerceModels;

namespace WooCommerceApi.Helpers
{
    public static class Converters
    {

        // Helper function to convert WebProduct to WooProduct
        public static WooProduct ConvertToWooProduct(WebProduct entity)
        {
            return new WooProduct
            {
                Id = entity.Id,
                Name = entity.Name,
                Slug = entity.Slug,
                DateCreated = entity.DateCreated,
                DateModified = entity.DateModified,
                Type = entity.Type,
                Status = entity.Status,
                Description = entity.Description,
                Sku = entity.Sku,
                Price = entity.Price,
                RegularPrice = entity.RegularPrice,
                SalePrice = entity.SalePrice,
                StockQuantity = entity.StockQuantity,
                Categories = entity.Categories.Select(c => new WooProductCategory() { Id = c.Id, Name = c.Name, Slug = c.Slug}).ToList(),
                Tags = entity.Tags,
                Images = entity.Images.Select(i => new WooProductImage
                {
                    Id = i.Id,
                    Src = i.Src,
                    Name = i.Name,
                    Alt = i.Alt
                }).ToList(),
                Attributes = entity.Attributes.Select(a => new WooProductAttribute
                {
                    Id = a.Id,
                    Name = a.Name,
                    Position = a.Position,
                    Visible = a.Visible,
                    Variation = a.Variation,
                    Options = a.Options
                }).ToList(),
                Variations = entity.Variations,
                // Additional Woo-specific properties
                Featured = false,
                CatalogVisibility = "visible",
                ShortDescription = "",  // Default
                OnSale = !string.IsNullOrEmpty(entity.SalePrice),
                StockStatus = entity.StockQuantity > 0 ? "instock" : "outofstock",
                ManageStock = true,
                BackordersAllowed = false,
                SoldIndividually = false,
                Weight = "0",
            };
        }

        // Helper function to convert WooProduct to WebProduct
        public static WebProduct ConvertToWebProduct(WooProduct woo)
        {
            return new WebProduct
            {
                Id = woo.Id,
                Name = woo.Name,
                Slug = woo.Slug,
                DateCreated = woo.DateCreated,
                DateModified = woo.DateModified,
                Type = woo.Type,
                Status = woo.Status,
                Description = woo.Description,
                Sku = woo.Sku,
                Price = woo.Price,
                RegularPrice = woo.RegularPrice,
                SalePrice = woo.SalePrice,
                StockQuantity = woo.StockQuantity ?? 0,
                Categories = woo.Categories.Select(c => new WebCategory
                {
                    Name = c.Name,
                }).ToList(),
                Tags = woo.Tags,
                Images = woo.Images.Select(i => new WebProductImage
                {
                    Id = i.Id,
                    Src = i.Src,
                    Name = i.Name,
                    Alt = i.Alt
                }).ToList(),
                Attributes = woo.Attributes.Select(a => new WebProductAttribute
                {
                    Id = a.Id,
                    Name = a.Name,
                    Position = a.Position,
                    Visible = a.Visible,
                    Variation = a.Variation,
                    Options = a.Options
                }).ToList(),
                Variations = woo.Variations
            };
        }

    }
}
