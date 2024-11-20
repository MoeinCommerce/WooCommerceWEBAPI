using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WebApi.Exceptions;
using WebApi.Models;
using WooCommerceApi.Contexts;
using WooCommerceApi.Utilities;

namespace WooCommerceApi
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string url = "https://testwp2.moeinapi.ir/";    // WooCommerce URL
            var apiConfigs = new Dictionary<string, string>()
            {
                { "WooCommerceUrl", "https://testwp2.moeinapi.ir/" },
                { "WooCommerceConsumerKey", Constants.CONSUMER_KEY },
                { "WooCommerceConsumerSecret", Constants.CONSUMER_SECRET },
            };
            var api = new WebContext(url, apiConfigs);
            try
            {
                var products = api.GetAllProductsWithFields(new List<string> { "id", "name" });
                foreach (var product in products)
                {
                    Console.WriteLine($"ProductId: {product.Id}, ProductName: {product.Name}");
                }
            }
            catch (DoesNotExistException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidFieldException e)
            {
                Console.WriteLine($"Field name: {e.Field} and content: {e.Content}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
