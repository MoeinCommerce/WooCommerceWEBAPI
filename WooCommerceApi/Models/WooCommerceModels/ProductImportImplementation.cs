using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;

namespace WooCommerceApi.Models.WooCommerceModels
{
    public class ProductImportImplementation : ProductImportBase
    {
        public override string Id => "id";

        public override string Name => "name";

        public override string CreatedDate => "date_created";

        public override string RegularPrice => "regular_price";

        public override string SalePrice => "sale_price";

        public override string Stock => "stock_quantity";

        public override string Category => "categories";
    }
}
