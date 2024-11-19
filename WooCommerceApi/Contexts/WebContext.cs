
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using WebApi.Contexts;
using WebApi.Contexts.Interfaces;
using WebApi.Exceptions;
using WebApi.Models;
using WooCommerceApi.Helpers;
using WooCommerceApi.Models.WooCommerceModels;
using Exception = System.Exception;

namespace WooCommerceApi.Contexts
{
    public class WebContext : BaseWebContext, IWebContext
    {
        private readonly RestClient  _client;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _maxPage = int.MaxValue;

        public WebContext(string url, Dictionary<string, string> configs) : base(configs)
        {
            try
            {
                var consumerKey = configs["WooCommerceConsumerKey"];
                var consumerSecret = configs["WooCommerceConsumerSecret"];
                const string path = "wp-json/wc/v3";
                _client = new RestClient(new Uri(new Uri(url), path));
                
                // Set default headers
                _client.AddDefaultHeader("Accept", "application/json");

                // Set authentication using WooCommerce API keys
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{consumerKey}:{consumerSecret}"));
                _client.AddDefaultHeader("Authorization", $"Basic {credentials}");
            }
            catch (Exception ex)
            {
                throw new InvalidFieldException(ex.Source, ex.Message);
            }
        }
        private Task<T> SendRequest<T>(RestRequest request, object body = null)
        {
            try
            {
                // If there is a body, serialize it to JSON and add it to the request
                if (body != null)
                {
                    var jsonBody = JsonConvert.SerializeObject(body);
                    request.AddJsonBody(jsonBody);
                }

                var response = _client.Execute(request);

                // Decode the response content
                var decodedContent = response.RawBytes != null
                    ? Encoding.UTF8.GetString(response.RawBytes) // Replace UTF8 if needed
                    : response.Content ?? string.Empty;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created: // Handle created status for POST requests
                        return Task.FromResult(JsonConvert.DeserializeObject<T>(decodedContent));
                    case HttpStatusCode.NotFound:
                        throw new DoesNotExistException();
                    case HttpStatusCode.BadRequest:
                        throw new InvalidFieldException("BadRequest", response.Content);
                    case HttpStatusCode.Unauthorized:
                        throw new InvalidFieldException("Unauthorized", response.Content);
                    default:
                        throw new InvalidFieldException($"Error! Status code: {response.StatusCode}", response.Content);
                }
            }
            catch (InvalidFieldException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HTTP Request error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves all entities using a custom processing function for each page.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="processPage">A function to process each page of results. It should return true if you want to continue fetching more pages, false to stop.</param>
        /// <returns>Enumerable of entities of type <typeparamref name="T"/>.</returns>
        private IEnumerable<T> GetAllWithPagination<T>(RestRequest request, Func<IEnumerable<T>, bool> processPage)
        {
            var results = new List<T>();
            while (true)
            {
                // Deserialize the response content into the expected entity type
                var result = SendRequest<IList<T>>(request).Result;
                if (result == null || !result.Any())
                {
                    // No more results, stop pagination
                    break;
                }

                // Add the results from this page to the overall collection
                results.AddRange(result);

                // Call the processing function for the current page
                // If the function returns false, stop fetching more pages
                if (!processPage(result) || _currentPage >= _maxPage)
                {
                    break;
                }

                // Move to the next page
                NextPage(request);
            }
            ResetPage(request);
            return results;
        }

        private void ResetPage(RestRequest request)
        {
            _currentPage = 1;
            request.AddOrUpdateParameter("size", _pageSize.ToString());
            request.AddOrUpdateParameter("page", _currentPage.ToString());
        }

        /// <summary>
        /// Advances to the next page in the paginated results.
        /// </summary>
        private void NextPage(RestRequest request)
        {
            _currentPage++;
            request.AddOrUpdateParameter("size", _pageSize.ToString());
            request.AddOrUpdateParameter("page", _currentPage.ToString());
        }

        /// <summary>
        /// Sets the endpoint with pagination and any additional filters using the QueryFilterBuilder.
        /// </summary>
        /// <param name="entity"></param>
        //private void SetParameters(string endpoint)
        //{
            //_queryBuilder
             //   .AddOr
            //_endpoint = $"wp-json/wc/v3/{endpoint}";
        //}UpdateParameter("page", _currentPage.ToString())
             //   .AddOrUpdateParameter("per_page", "100");

        public new int CreateProduct(WebProduct entity)
        {
            const string endpoint = "products";
            var wooProduct = WooCommerceConverters.ToWooProduct(entity);
            var request = new RestRequest(endpoint, Method.Post);
            var createdProduct = SendRequest<WooProduct>(request, wooProduct).Result;
            return WooCommerceConverters.TryToInt(createdProduct.Id);
        }
        public new int UpdateProduct(int id, WebProduct entity)
        {
            var existingProduct = GetProductById(id);
            if (existingProduct == null)
            {
                throw new DoesNotExistException();
            }
            var endpoint = $"products/{id}";
            var request = new RestRequest(endpoint, Method.Put);
            var updatedProductData = WooCommerceConverters.ToWooProduct(entity);
            var updatedProduct = SendRequest<WooProduct>(request, updatedProductData);

            return WooCommerceConverters.TryToInt(updatedProduct.Id);
        }

        public new WebProduct GetProductById(int id)
        {
            var endpoint = $"products/{id}";
            var request = new RestRequest(endpoint, Method.Get);
            var wooProduct = SendRequest<WooProduct>(request).Result;
            return WooCommerceConverters.ToWebProduct(wooProduct);
        }

        public new IEnumerable<WebProduct> GetAllProducts()
        {
            const string endpoint = "products";
            var request = new RestRequest(endpoint, Method.Get);    
            var results = new List<WooProduct>();
            return GetAllWithPagination<WooProduct>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Select(WooCommerceConverters.ToWebProduct).ToList();
        }

        public new int GetTotalProductsCount(string searchTerm)
        {
            const string endpoint = "products";
            var request = new RestRequest(endpoint, Method.Get);
            _maxPage = int.MaxValue;
            _pageSize = 100;
            ResetPage(request);
            if (searchTerm != null)
            {
                request.AddParameter("search", searchTerm);
            }
            var results = new List<WooProduct>();
            return GetAllWithPagination<WooProduct>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Count();
        }

        public new IEnumerable<WebProduct> GetAllProductsExcludingIds(IList<int> idsToExclude)
        {
            const string endPoint = "products";
            var idsToExcludeString = string.Join(",", idsToExclude);
            var request = new RestRequest(endPoint, Method.Get);
            request.AddParameter("exclude", idsToExcludeString);
            var products = SendRequest<IList<WooProduct>>(request).Result;
            return products.Select(WooCommerceConverters.ToWebProduct)
                .Where(p => !idsToExclude.Contains(p.Id)).ToList(); 
        }
        public new IEnumerable<WebProduct> SearchProducts(string searchTerm, int page = 1, int pageSize = 10, int maxPage = 1)
        {
            _currentPage = page < 1 ? 1 : page;
            _pageSize = pageSize > 100 || pageSize < 1 ? 100 : pageSize;
            _maxPage = maxPage < 1 ? 1 : maxPage;
            const string endPoint = "products";
            var request = new RestRequest(endPoint, Method.Get);
            if (searchTerm == null)
            {
                return new List<WebProduct>();
            }
            
            request.AddParameter("search", searchTerm);
            var results = new List<WooProduct>();
            return GetAllWithPagination<WooProduct>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Select(WooCommerceConverters.ToWebProduct).ToList();
        }
        public new IEnumerable<WebProduct> GetAllProductsWithFields(IList<string> fields)
        {
            if (fields == null || !fields.Any())
            {
                return new List<WebProduct>();
            }
            const string endPoint = "products";
            var fieldsString = string.Join(",", fields);
            var request = new RestRequest(endPoint, Method.Get);
            request.AddParameter("_fields", fieldsString);
            var results = new List<WooProduct>();
            return GetAllWithPagination<WooProduct>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Select(WooCommerceConverters.ToWebProduct).ToList();
        }

        public new int GetMaxProductId()
        {
            const string endpoint = "products";
            var request = new RestRequest(endpoint, Method.Get);
            var products = SendRequest<IList<WooProduct>>(request).Result;
            return products.Max(product => WooCommerceConverters.TryToInt(product.Id));
        }

        public new WebCategory GetCategoryById(int id)
        { 
            var endPoint = $"products/categories/{id}";
            var request = new RestRequest(endPoint, Method.Get);
            var wooCategory = SendRequest<WooCategory>(request).Result;
            return WooCommerceConverters.ToWebCategory(wooCategory);
        }

        public new IEnumerable<WebCategory> GetAllCategories()
        {
            throw new System.NotImplementedException();
        }

        public new IList<WebCategory> GetAllCategoriesExcludingIds(IEnumerable<int> idsToExclude)
        {
            throw new System.NotImplementedException();
        }

        public new int CreateCategory(WebCategory entity)
        {
            const string endPoint = "products/categories";
            var wooCategory = WooCommerceConverters.ToWooCategory(entity);
            var request = new RestRequest(endPoint, Method.Post);
            var createdCategory = SendRequest<WooCategory>(request, wooCategory).Result;
            return WooCommerceConverters.TryToInt(createdCategory.Id);
        }

        public new int UpdateCategory(int id, WebCategory entity)
        {
            var existingCategory = GetCategoryById(id);
            if (existingCategory == null)
            {
                throw new DoesNotExistException();
            }
            var endPoint = $"products/categories/{id}";
            var request = new RestRequest(endPoint, Method.Put);
            var updatedCategoryData = WooCommerceConverters.ToWooCategory(entity);
            var updatedCategory = SendRequest<WooCategory>(request, updatedCategoryData);
            return WooCommerceConverters.TryToInt(updatedCategory.Id);
        }

        public new IList<WebCategory> GetAllCategoriesWithFields(IList<string> fields)
        {
            if (fields == null || !fields.Any())
            {
                return new List<WebCategory>();
            }
            const string endPoint = "products/categories";
            var fieldsString = string.Join(",", fields);
            var request = new RestRequest(endPoint, Method.Get);
            request.AddParameter("_fields", fieldsString);
            var results = new List<WooCategory>();
            return GetAllWithPagination<WooCategory>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Select(WooCommerceConverters.ToWebCategory).ToList();
        }

        public new int GetMaxCategoryId()
        {
            throw new System.NotImplementedException();
        }
        public new IList<WebCategory> SearchCategories(string searchTerm, int page = 1, int pageSize = 10, int maxPage = 1)
        {
            _currentPage = page < 1 ? 1 : page;
            _pageSize = pageSize > 100 || pageSize < 1 ? 100 : pageSize;
            _maxPage = maxPage < 1 ? 1 : maxPage;
            const string endPoint = "products/categories";
            var request = new RestRequest(endPoint, Method.Get);
            if (searchTerm == null)
            {
                return new List<WebCategory>();
            }
            
            request.AddParameter("search", searchTerm);
            var results = new List<WooCategory>();
            return GetAllWithPagination<WooCategory>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Select(WooCommerceConverters.ToWebCategory).ToList();
        }

        public new void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
