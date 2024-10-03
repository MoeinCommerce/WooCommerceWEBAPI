using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models;
using WooCommerceApi.Models.WooCommerceModels;

namespace WooCommerceApi.Helpers
{
    public static class Converters
    {
        // Helper function to convert WebProduct to WooProduct
        public static WooProduct ConvertToWooProduct(WebProduct entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new WooProduct
            {
                Name = entity.Name ?? string.Empty,
                Slug = entity.Slug ?? string.Empty,
                DateCreated = entity.DateCreated != DateTime.MinValue ?
                    entity.DateCreated.ToString("yyyy-MM-ddTHH:mm:ss") : string.Empty,
                DateCreatedGmt = entity.DateCreated.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"),
                Type = entity.Type ?? "simple", // Ensure it's a valid type, default to "simple"

                // Validate Status
                Status = IsValidStatus(entity.Status) ? entity.Status : "draft", // Default to "draft" if invalid

                Description = entity.Description ?? string.Empty,
                Sku = entity.Sku ?? string.Empty,
                RegularPrice = entity.RegularPrice ?? string.Empty,
                SalePrice = entity.SalePrice ?? string.Empty,
                StockQuantity = entity.StockQuantity.ToString() ?? "0",
                Categories = entity.Categories?.Select(c => new WooProductCategory
                {
                    Id = c.Id,
                    Name = c.Name ?? string.Empty,
                    Slug = c.Slug ?? string.Empty
                }).ToList() ?? new List<WooProductCategory>(),
                Tags = entity.Tags != null && entity.Tags.Any() ? string.Join(",", entity.Tags) : string.Empty,
                Images = entity.Images?.Select(i => new WooProductImage
                {
                    Id = i.Id,
                    Src = i.Src ?? string.Empty,
                    Name = i.Name ?? string.Empty,
                    Alt = i.Alt ?? string.Empty
                }).ToList() ?? new List<WooProductImage>(),
                Attributes = entity.Attributes?.Select(a => new WooProductAttribute
                {
                    Id = a.Id,
                    Name = a.Name ?? string.Empty,
                    Position = a.Position,
                    Visible = a.Visible,
                    Variation = a.Variation,
                    Options = a.Options ?? new List<string>()
                }).ToList() ?? new List<WooProductAttribute>(),
                //// Ensure valid catalog visibility
                //CatalogVisibility = IsValidCatalogVisibility(entity.CatalogVisibility)
                //           ? entity.CatalogVisibility
                //           : "visible", // Default to "visible"
            };
        }

        // Helper method to validate the status
        private static bool IsValidStatus(string status)
        {
            var validStatuses = new List<string> { "draft", "pending", "private", "publish", "future", "auto-draft", "trash" };
            return validStatuses.Contains(status);
        }
        // Helper method to validate catalog visibility
        private static bool IsValidCatalogVisibility(string visibility)
        {
            var validCatalogVisibilities = new List<string> { "visible", "catalog", "search", "hidden" };
            return validCatalogVisibilities.Contains(visibility);
        }

        // Helper function to convert WooProduct to WebProduct
        public static WebProduct ConvertToWebProduct(WooProduct woo)
        {
            return new WebProduct
            {
                Name = woo.Name,
                Slug = woo.Slug,
                DateCreated = DateTime.Parse(woo.DateCreated), // Assuming valid ISO date format
                DateModified = DateTime.UtcNow, // This can be populated based on actual data if available
                Type = woo.Type,
                Status = woo.Status,
                Description = woo.Description,
                Sku = woo.Sku,
                RegularPrice = woo.RegularPrice,
                SalePrice = woo.SalePrice,
                StockQuantity = int.TryParse(woo.StockQuantity, out int qty) ? qty : 0,
                Categories = woo.Categories?.Select(c => new WebCategory
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug
                }).ToList(),
                Tags = woo.Tags?.Split(',').ToList(),
                Images = woo.Images?.Select(i => new WebProductImage
                {
                    Id = i.Id,
                    Src = i.Src,
                    Name = i.Name,
                    Alt = i.Alt
                }).ToList(),
                Attributes = woo.Attributes?.Select(a => new WebProductAttribute
                {
                    Id = a.Id,
                    Name = a.Name,
                    Position = a.Position,
                    Visible = a.Visible,
                    Variation = a.Variation,
                    Options = a.Options
                }).ToList(),
            };
        }
    }
}
