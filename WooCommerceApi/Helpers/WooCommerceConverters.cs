using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
                Id = webProduct.Id,
                Name = webProduct.Name,
                Slug = webProduct.Slug,
                Description = webProduct.Description,
                RegularPrice = webProduct.RegularPrice.ToString(CultureInfo.InvariantCulture),
                SalePrice = webProduct.SalePrice?.ToString(),
                ManageStock = "true",
                StockQuantity = webProduct.StockQuantity.ToString(),
                Sku = webProduct.Sku,
                Categories = webProduct.Categories?.Select(c => new MinimalWooCategory
                {
                    Id = c.Id
                }).ToList()
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
                SalePrice = TryToNullableDecimal(wooProduct.SalePrice),
                StockQuantity = TryToInt(wooProduct.StockQuantity),
                Categories =  wooProduct.Categories?.Select(c => new WebCategory
                {
                    Id = c.Id
                }).ToList()
            };
        }

        public static WooCategory ToWooCategory(WebCategory webCategory)
        {
            return new WooCategory
            {
                Id = webCategory.Id,
                Name = webCategory.Name,
                Description = webCategory.Description,
                Parent = webCategory.ParentId,
                Slug = webCategory.Name.GenerateSlug()
            };
        }
        
        public static WebCustomer ToWebCustomer(WooCustomer wooCustomer)
        {
            return new WebCustomer
            {
                Id = wooCustomer.Id,
                FirstName = wooCustomer.FirstName,
                LastName = wooCustomer.LastName,
                Email = wooCustomer.Email,
                PhoneNumbers = new List<string>
                {
                    wooCustomer.Phone  
                },
                Address1 = wooCustomer.Address1,
                Address2 = wooCustomer.Address2,
                City = wooCustomer.City,
                State = wooCustomer.State,
                Postcode = wooCustomer.Postcode,
                Country = wooCustomer.Country,
                CreatedDate = wooCustomer.DateCreated ?? DateTime.Now,
            };
        }

        private static int? TryToNullableInt(object value)
        {
            int.TryParse(value?.ToString(), out int result);
            if (result == 0)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public static WebCategory ToWebCategory(WooCategory wooCategory)
        {
            return new WebCategory
            {
                Id = TryToNullableInt(wooCategory.Id) ?? 0,  // Assigns 0 if Id is null
                Name = wooCategory.Name,
                Description = wooCategory.Description,
                ParentId = TryToNullableInt(wooCategory.Parent)
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
        internal static decimal? TryToNullableDecimal(object value)
        {
            return decimal.TryParse(value?.ToString(), out var result) ? result : (decimal?)null;
        }
        public static string GenerateSlug(this string phrase) 
        { 
            string str = phrase.RemoveAccent().ToLower(); 
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); 
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim(); 
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();   
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str; 
        } 

        public static string RemoveAccent(this string txt) 
        { 
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt); 
            return System.Text.Encoding.ASCII.GetString(bytes); 
        }
    }
}