using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Contexts;
using WebApi.Models;
using WebApi.Repositories.Entities;
using WooCommerceApi.Repositories.Entities;
using WooCommerceApi.Repositories.WooRepositories;
using WooCommerceApi.Repositories.WooRepositories.Interfaces;

namespace WooCommerceApi.Contexts
{
    public class WebContext : IWebContext
    {
        private readonly Dictionary<string, string> _apiConfigs;
        protected IRestClient _httpClient;
        private IProductRepository _webProductRepository;
        private IOrderRepository _webOrderRepository;
        private ICategoryRepository _webCategoryRepository;

        public WebContext(Dictionary<string, string> apiConfigs)
        {
            _apiConfigs = apiConfigs;
        }

        public IProductRepository WebProductRepository
        {
            get
            {
                if (_webProductRepository  == null)
                {
                    IWooProductRepository wooProduct = new WooProductRepository(_apiConfigs);
                    _webProductRepository = new WebProductRepository(_apiConfigs, "products", wooProduct);
                }
                return _webProductRepository;
            }
        }

        public IOrderRepository WebOrderRepository
        {
            get
            {
                if (_webOrderRepository == null)
                {
                    IWooOrderRepository wooOrder = new WooOrderRepository(_apiConfigs);
                    _webOrderRepository = new WebOrderRepository(wooOrder);
                }
                return _webOrderRepository;
            }
        }

        public ICategoryRepository WebCategoryRepository
        {
            get
            {
                if (_webCategoryRepository == null)
                {
                    IWooCategoryRepository wooCategory = new WooCategoryRepository(_apiConfigs);
                    _webCategoryRepository = new WebCategoryRepository(wooCategory);
                }
                return _webCategoryRepository;
            }
        }

        #region IDisposable Implementation
        protected virtual void Dispose(bool disposing) { }

        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }

        ~WebContext()
        {
            Dispose(false);
        }
        #endregion
    }
}
