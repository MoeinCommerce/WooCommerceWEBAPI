using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Repositories.Entities;
using WooCommerceApi.Models.WooCommerceModels;
using WooCommerceApi.Repositories.WooRepositories.Interfaces;

namespace WooCommerceApi.Repositories.WooRepositories
{
    public class WooOrderRepository : WebRepository<WooOrder>, IWooOrderRepository
    {
        public WooOrderRepository(Dictionary<string, string> _apiConfigs)
            : base(_apiConfigs, "orders")
        {
        }
    }
}
