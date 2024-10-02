using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi;

namespace WooCommerceApi
{
    public class Information : IWebInformation
    {
        public string Name => "WooCommerceApi";

        public string DisplayName => "WooCommerceApi";

        public string Description => "This is a dll that stores all wooCommerce api";

        public string ExecutablePath => AppDomain.CurrentDomain.BaseDirectory;

        public string Version => "1.0.0";
        public Dictionary<string, string> Configurations { get; set; }
    }
}
