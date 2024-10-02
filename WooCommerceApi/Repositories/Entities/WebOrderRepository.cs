using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Repositories.Entities;
using WooCommerceApi.Repositories.WooRepositories.Interfaces;

namespace WooCommerceApi.Repositories.Entities
{
    public class WebOrderRepository : IOrderRepository
    {
        private readonly IWooOrderRepository _wooOrderRepository;
        public WebOrderRepository(IWooOrderRepository wooOrderRepository)
        {
            _wooOrderRepository = wooOrderRepository;
        }
        public void Create(WebOrder entity)
        {
            throw new NotImplementedException();
        }

        public string Delete(int id)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<WebOrder> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WebOrder> GetAllExcludingIds(IEnumerable<int> idsToExclude)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WebOrder> GetAllWithFields(IEnumerable<string> fields)
        {
            throw new NotImplementedException();
        }

        public WebOrder GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, WebOrder entity)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
