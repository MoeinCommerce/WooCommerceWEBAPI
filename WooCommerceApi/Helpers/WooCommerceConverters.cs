using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using WebApi.Models;
using WooCommerceApi.Models.WooCommerceModels;
using WooCommerceApi.Utilities;

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
                SalePrice = webProduct.SalePrice?.ToString(CultureInfo.InvariantCulture),
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
                // Status = wooProduct.Status,
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

        public static WebOrder ToWebOrder(WooOrder wooOrder)
        {
            OrderStatus orderStatus = Constants.OrderStatuses.FirstOrDefault(x => x.Value == wooOrder.Status).Key;
            Constants.PaymentMethods.TryGetValue(wooOrder.PaymentMethod, out var paymentMethodIntId);

            double totalTax = wooOrder.TotalTax;
            double sumOfSubTaxes = wooOrder.LineItems.Sum(x => x.SubtotalTax);
            bool isTaxAfterDiscount = totalTax != sumOfSubTaxes;

            return new WebOrder
            {
                Id = wooOrder.Id,
                CustomerId = wooOrder.CustomerId,
                PaymentMethodId = paymentMethodIntId,
                TransactionId = wooOrder.TransactionId,
                Status = orderStatus,
                DateCreated = wooOrder.DateCreated ?? DateTime.Now,
                DateModified = wooOrder.DateModified ?? DateTime.Now,
                Currency = wooOrder.Currency,
                ShippingTotal = wooOrder.ShippingTotal,
                // OrderTax = wooOrder.TotalTax,
                Billing = new WebCustomer
                {
                    FirstName = wooOrder.Billing.FirstName,
                    LastName = wooOrder.Billing.LastName,
                    Email = wooOrder.Billing.Email,
                    PhoneNumbers = new List<string>
                    {
                        wooOrder.Billing.Phone
                    },
                    Address1 = wooOrder.Billing.Address1,
                    Address2 = wooOrder.Billing.Address2,
                    City = wooOrder.Billing.City,
                    State = wooOrder.Billing.State,
                    Postcode = wooOrder.Billing.Postcode,
                    Country = wooOrder.Billing.Country
                },
                Shipping = new WebCustomer
                {
                    FirstName = wooOrder.Shipping.FirstName,
                    LastName = wooOrder.Shipping.LastName,
                    Email = wooOrder.Shipping.Email,
                    PhoneNumbers = new List<string>
                    {
                        wooOrder.Shipping.Phone
                    },
                    Address1 = wooOrder.Shipping.Address1,
                    Address2 = wooOrder.Shipping.Address2,
                    City = wooOrder.Shipping.City,
                    State = wooOrder.Shipping.State,
                    Postcode = wooOrder.Shipping.Postcode,
                    Country = wooOrder.Shipping.Country
                },
                OrderItems = wooOrder.LineItems.Select(item => new WebOrderDetail
                {
                    Name = item.Name,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Quantity == 0 ? item.Subtotal : item.Subtotal / item.Quantity,
                    VariationId = item.VariationId,
                    UnitDiscount = ((item.Subtotal - item.Total) / item.Quantity),
                    UnitTax = (isTaxAfterDiscount ? item.TotalTax : item.SubtotalTax) / item.Quantity
                }).ToList()
            };
        }
        public static WebPaymentMethod ToWebPaymentMethod(WooPaymentMethod wooPaymentMethod)
        {
            Constants.PaymentMethods.TryGetValue(wooPaymentMethod.Id, out var paymentMethodIntId);
            return new WebPaymentMethod
            {
                Id = paymentMethodIntId,
                Title = wooPaymentMethod.Title,
                Description = wooPaymentMethod.Description
            };
        }
        internal static int TryToInt(object value)
        {
            return int.TryParse(value?.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : 0;
        }

        internal static decimal TryToDecimal(object value)
        {
            return decimal.TryParse(value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0m;
        }

        internal static decimal? TryToNullableDecimal(object value)
        {
            return decimal.TryParse(value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : (decimal?)null;
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