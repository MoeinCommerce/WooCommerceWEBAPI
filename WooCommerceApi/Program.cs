using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceApi.Contexts;
using WooCommerceApi.Utilities;

namespace WooCommerceApi
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> apiConfigs = new Dictionary<string, string>()
            {
                { "BaseUrl", "https://testwp2.moeinapi.ir/" },
                { "ConsumerKey", Constants.CONSUMER_KEY },
                { "ConsumerSecret", Constants.CONSUMER_SECRET },
            };
            var api = new WebContext(apiConfigs);

            var result = api.WebProductRepository.GetAllWithFields(new List<string>() { "id", "name"}); 
            Console.WriteLine("Hi from Erfan.");
            Console.ReadKey();
        }
    }
}
