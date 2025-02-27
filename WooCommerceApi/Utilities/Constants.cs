using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebApi.Models;

namespace WooCommerceApi.Utilities
{
    public class Constants
    {
        public static string CONSUMER_KEY = "ck_0b65361a66a4784f7fa6c1f7078e50f9e6521516";
        public static string CONSUMER_SECRET = "cs_8e1d1d6c0439c5a0d0452c886b1485b5bb505280";
        public static Dictionary<string, int> PaymentMethods = new Dictionary<string, int>
        {
            {"cod", 1},
            {"bacs", 2},
            {"cheque", 3},
            {"paypal", 4},
            {"stripe", 5},
            {"bankmellat", 6},
            {"WC_ZPal", 7},
            {"WC_IDPay", 8}
        };
        public static Dictionary<ProductTypes, string> ProductTypes = new Dictionary<ProductTypes, string>
        {
            {WebApi.Models.ProductTypes.Simple, "simple" },
            {WebApi.Models.ProductTypes.Variable, "variable" },
            {WebApi.Models.ProductTypes.Variation, "variation" },
        };
        public static Dictionary<OrderStatus, string> OrderStatuses = new Dictionary<OrderStatus, string>
        {
            {WebApi.Models.OrderStatus.Pending, "pending" },
            {WebApi.Models.OrderStatus.Processing, "processing" },
            {WebApi.Models.OrderStatus.OnHold, "on-hold" },
            {WebApi.Models.OrderStatus.Completed, "completed" },
            {WebApi.Models.OrderStatus.Cancelled, "cancelled" },
            {WebApi.Models.OrderStatus.Refunded, "refunded" },
            {WebApi.Models.OrderStatus.Failed, "failed" },
            {WebApi.Models.OrderStatus.Other, "others" }
        };
    }
}
