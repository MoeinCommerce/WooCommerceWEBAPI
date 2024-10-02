using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Repositories.Entities;
using WooCommerceApi.Helpers;
using WooCommerceApi.Models.WooCommerceModels;
using WooCommerceApi.Repositories.WooRepositories.Interfaces;

namespace WooCommerceApi.Repositories.Entities
{
    public class WebProductRepository : WebRepository<WebProduct>, IProductRepository
    {
        private readonly IWooProductRepository _wooProductRepository;
        public WebProductRepository(
            Dictionary<string, string> apiConfigs,
            string endpoint,
            IWooProductRepository wooProductRepository) 
            : base(apiConfigs, endpoint)
        {
            _wooProductRepository = wooProductRepository;
        }

        public override void Create(WebProduct entity)
        {
            // Convert WebProduct to WooProduct using the helper function
            WooProduct wooProduct = Converters.ConvertToWooProduct(entity);
            _wooProductRepository.Create(wooProduct);
        }

        public override string Delete(int id)
        {
            string response = _wooProductRepository.Delete(id);
            return response;
        }

        public override IEnumerable<WebProduct> GetAll()
        {
            // Retrieve WooProducts and convert them to WebProducts
            var wooProducts = _wooProductRepository.GetAll();
            return wooProducts.Select(Converters.ConvertToWebProduct).ToList();
        }

        public override IEnumerable<WebProduct> GetAllExcludingIds(IEnumerable<int> idsToExclude)
        {
            // Retrieve WooProducts excluding certain IDs and convert them to WebProducts
            var wooProducts = _wooProductRepository.GetAllExcludingIds(idsToExclude);
            return wooProducts.Select(Converters.ConvertToWebProduct).ToList();
        }

        public override IEnumerable<WebProduct> GetAllWithFields(IEnumerable<string> fields)
        {
            // Retrieve WooProducts excluding certain IDs and convert them to WebProducts
            var wooProducts = _wooProductRepository.GetAllWithFields(fields);
            return wooProducts.Select(Converters.ConvertToWebProduct).ToList();
        }

        public override WebProduct GetById(int id)
        {
            // Retrieve WooProduct by Id and convert it to WebProduct
            var wooProduct = _wooProductRepository.GetById(id);
            if (wooProduct == null)
            {
                return null;
            }
            return Converters.ConvertToWebProduct(wooProduct);
        }

        public override void Update(int id, WebProduct entity)
        {
            // Convert WebProduct to WooProduct and update
            WooProduct wooProduct = Converters.ConvertToWooProduct(entity);
            _wooProductRepository.Update(id, wooProduct);
        }
    }
}
