using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebApi.Models;
using WooCommerceApi.Models.WooCommerceModels;

namespace WooCommerceApi.Helpers
{
    public static class WooCommerceConverters
    {
        public static WooProduct ToWooProduct(WebProduct webProduct)
        {
            return new WooProduct
            {
                Name = webProduct.Name,
                Slug = webProduct.Slug,
                Description = webProduct.Description,
                RegularPrice = webProduct.RegularPrice.ToString(CultureInfo.InvariantCulture),
                SalePrice = webProduct.SalePrice.ToString(CultureInfo.InvariantCulture),
                StockQuantity = webProduct.StockQuantity.ToString(),
                Sku = webProduct.Sku,
                Categories = webProduct.Categories?.Select(c => new WooCategory
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),
            };
        }

        public static WebProduct ToWebProduct(WooProduct wooProduct)
        {
            return new WebProduct
            {
                Id = TryToInt(wooProduct.Id),
                Name = wooProduct.Name,
                Slug = wooProduct.Slug,
                DateCreated = wooProduct.DateCreated ?? DateTime.Now,
                DateModified = wooProduct.DateModified ?? DateTime.Now,
                Status = wooProduct.Status,
                Description = wooProduct.Description,
                Sku = wooProduct.Sku,
                RegularPrice = TryToDecimal(wooProduct.RegularPrice),
                SalePrice = TryToDecimal(wooProduct.SalePrice),
                StockQuantity = TryToInt(wooProduct.StockQuantity),
                Categories = wooProduct.Categories?.Select(c => new WebCategory
                {
                    Id = TryToInt(c.Id),
                    Name = c.Name
                }).ToList(),
            };
        }

        public static WooCategory ToWooCategory(WebCategory webCategory)
        {
            return new WooCategory
            {
                Name = webCategory.Name,
                Description = webCategory.Description,
                Parent = webCategory.ParentId,
            };
        }

        public static WebCategory ToWebCategory(WooCategory wooCategory)
        {
            return new WebCategory
            {
                Id = TryToInt(wooCategory.Id),
                Name = wooCategory.Name,
                Description = wooCategory.Description,
                ParentId = TryToInt(wooCategory.Parent),
            };
        }
        internal static int TryToInt(object value)
        {
            return int.TryParse(value?.ToString(), out var result) ? result : 0;
        }

        internal static decimal TryToDecimal(object value)
        {
            return decimal.TryParse(value?.ToString(), out var result) ? result : 0m;
        }
    }
}