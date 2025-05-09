using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using WebApi.Models;
using WooCommerceApi.Models.WooCommerceModels;
using WooCommerceApi.Utilities;
using System.Web;

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
                Description = webProduct.Description,
                RegularPrice = webProduct.RegularPrice.ToString(CultureInfo.InvariantCulture),
                SalePrice = webProduct.SalePrice?.ToString(CultureInfo.InvariantCulture),
                ManageStock = "true",
                StockQuantity = webProduct.StockQuantity.ToString(),
                Sku = webProduct.Sku,
                Categories = webProduct.Categories?.Select(c => new MinimalWooCategory
                {
                    Id = c.Id
                }).ToList(),
                Status = "draft"
            };
        }

        public static WebProduct ToWebProduct(WooProduct wooProduct)
        {
            return new WebProduct
            {
                Id = TryToInt(wooProduct.Id),
                Name = wooProduct.Name,
                DateCreated = wooProduct.DateCreated ?? DateTime.Now,
                DateModified = wooProduct.DateModified ?? DateTime.Now,
                // Status = wooProduct.Status,
                Description = wooProduct.Description,
                Sku = wooProduct.Sku,
                RegularPrice = TryToDecimal(wooProduct.RegularPrice),
                SalePrice = TryToNullableDecimal(wooProduct.SalePrice),
                StockQuantity = TryToDouble(wooProduct.StockQuantity),
                Categories =  wooProduct.Categories?.Select(c => new WebCategory
                {
                    Id = c.Id
                }).ToList(),
                Attributes = wooProduct?.Attributes?.Select(w => new WebApi.Models.Attribute
                {
                    Id = w.Id,
                    Name = HttpUtility.UrlDecode(w.Name),
                    Value = HttpUtility.UrlDecode(w.Option)
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
            var orderStatus = Constants.OrderStatuses
                .FirstOrDefault(x => x.Value == wooOrder.Status)
                .Key;
            if (!Constants.OrderStatuses.Any(o => o.Value == wooOrder.Status))
            {
                orderStatus = OrderStatus.Other;
            }


            Constants.PaymentMethods.TryGetValue(wooOrder.PaymentMethod, out var paymentMethodIntId);

            double totalTax = wooOrder.TotalTax;
            double sumOfSubTaxes = wooOrder.LineItems.Sum(x => x.SubtotalTax);
            bool isTaxAfterDiscount = totalTax != sumOfSubTaxes;
            
            var firstShippingLine = wooOrder.ShippingLines?.FirstOrDefault();

            return new WebOrder
            {
                Id = wooOrder.Id,
                OrderPath = string.Format(Constants.OrderPath, wooOrder.Id),
                CustomerId = wooOrder.CustomerId,
                CustomerNote = wooOrder.CustomerNote,
                PaymentMethod = new WebPaymentMethod
                {
                    Id = paymentMethodIntId,
                    Title = wooOrder.PaymentMethodTitle,
                },
                TransactionId = wooOrder.TransactionId,
                Status = orderStatus,
                StatusText = wooOrder.Status,
                IsConverted = false,
                DateCreated = wooOrder.DateCreated ?? DateTime.Now,
                DateModified = wooOrder.DateModified ?? DateTime.Now,
                Currency = wooOrder.Currency,
                ShippingTotal = wooOrder.ShippingTotal,
                Billing = MapCustomer(wooOrder.Billing),
                Shipping = MapCustomer(wooOrder.Shipping),
                ShippingDetail = new WebShippingDetail
                {
                    VehicleId = int.TryParse(firstShippingLine?.MethodId, out int methodId) ? methodId : 0,
                    VehicleName = firstShippingLine?.MethodTitle,
                    VehiclePrice = double.TryParse(firstShippingLine?.Total, out double shippingTotal) ? shippingTotal : 0
                },
                OrderItems = wooOrder.LineItems.Select(item => new WebOrderDetail
                {
                    Name = item.Name,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Quantity == 0 ? item.Subtotal : item.Subtotal / item.Quantity,
                    VariationId = item.VariationId,
                    UnitDiscount = (item.Subtotal - item.Total) / item.Quantity,
                    UnitTax = (isTaxAfterDiscount ? item.TotalTax : item.SubtotalTax) / item.Quantity
                }).ToList()
            };
        }
        private static WebCustomer MapCustomer(WooCustomer customer)
        {
            // Get country name from dictionary, if not found assign the original value
            string country = Constants.CountryDictionary.TryGetValue(customer.Country, out string countryName)
                             ? countryName : customer.Country;

            // Get state name from dictionary, if not found assign the original value
            string state = Constants.StateDictionary.TryGetValue(customer.State, out string stateName)
                           ? stateName : customer.State;

            return new WebCustomer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumbers = new List<string> { customer.Phone },
                Address1 = customer.Address1,
                Address2 = customer.Address2,
                Country = country,
                State = state,
                City = customer.City,
                Postcode = customer.Postcode
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
        internal static double TryToDouble(object value)
        {
            return double.TryParse(value?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0;
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