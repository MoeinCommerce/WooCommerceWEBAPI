using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi;
using WebApi.Models;

namespace WooCommerceApi
{
    public class WebInformation : IWebInformation
    {
        public string Name => "WooCommerceApi";

        public string DisplayName => "WooCommerceApi";

        public string Description => "This is a dll that stores all wooCommerce api";

        public string ExecutablePath => AppDomain.CurrentDomain.BaseDirectory;

        public string Version => "1.0.0";
        public string IconPath => string.Empty;

        public List<WebConfig> Configurations =>
            new List<WebConfig>
            {
                new WebConfig
                {
                    Key = "WooCommerceUrl",
                    DefaultValue = "https://example.com",
                    DisplayName = "WooCommerce Url"
                },
                new WebConfig
                {
                    Key = "WooCommerceConsumerKey",
                    DefaultValue = "ck_XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
                    DisplayName = "WooCommerce Consumer Key"
                },
                new WebConfig
                {
                    Key = "WooCommerceConsumerSecret",
                    DefaultValue = "cs_XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
                    DisplayName = "WooCommerce Consumer Secret"
                }
            };
    }
}
