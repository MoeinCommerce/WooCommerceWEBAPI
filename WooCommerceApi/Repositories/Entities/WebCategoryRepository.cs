using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Repositories.Entities;
using WooCommerceApi.Repositories.WooRepositories;
using WooCommerceApi.Repositories.WooRepositories.Interfaces;

namespace WooCommerceApi.Repositories.Entities
{
    public class WebCategoryRepository : ICategoryRepository
    {
        private readonly IWooCategoryRepository _wooCategoryRepository;

        public WebCategoryRepository(IWooCategoryRepository wooCategoryRepository) 
        {
            _wooCategoryRepository = wooCategoryRepository;
        }  
        public void Create(WebCategory entity)
        {
            throw new NotImplementedException();
        }

        public string Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WebCategory> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WebCategory> GetAllExcludingIds(IEnumerable<int> idsToExclude)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WebCategory> GetAllWithFields(IEnumerable<string> fields)
        {
            throw new NotImplementedException();
        }

        public WebCategory GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(int id, WebCategory entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
