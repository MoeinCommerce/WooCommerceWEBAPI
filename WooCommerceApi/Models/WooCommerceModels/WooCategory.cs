using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooCommerceApi.Models.WooCommerceModels
{
    public class WooCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public int Parent { get; set; }

        public string Description { get; set; }
        
        public int Count { get; set; }
    }

    public class WooCategoryImage
    {
        public int Id { get; set; }

        public string Src { get; set; }

        public string Name { get; set; }

        public string Alt { get; set; }
    }

    public class WooCategoryLinks
    {
        public WooCategoryLink[] Self { get; set; }

        public WooCategoryLink[] Collection { get; set; }
    }

    public class WooCategoryLink
    {
        public string Href { get; set; }
    }
}
