using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Repositories;
using WooCommerceApi.Models.WooCommerceModels;

namespace WooCommerceApi.Repositories.WooRepositories.Interfaces
{
    public interface IWooCategoryRepository : IWebRepository<WooCategory>
    {
    }
}
