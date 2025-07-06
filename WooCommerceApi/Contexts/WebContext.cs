﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using WebApi.Contexts;
using WebApi.Contexts.Interfaces;
using WebApi.Exceptions;
using WebApi.Models;
using WooCommerceApi.Helpers;
using WooCommerceApi.Models.WooCommerceModels;
using WooCommerceApi.Utilities;
using Exception = System.Exception;
   
namespace WooCommerceApi.Contexts
{
    public class WebContext : BaseWebContext, IWebContext
    {
        private readonly RestClient  _client;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _maxPage = int.MaxValue;

        public WebContext(string url, Dictionary<string, string> configs) : base(url, configs)
        {
            try
            {
                var consumerKey = configs["WooCommerceConsumerKey"];
                var consumerSecret = configs["WooCommerceConsumerSecret"];
                const string path = "wp-json/wc/v3";

                var options = new RestClientOptions(new Uri(new Uri(url), path))
                {
                    Authenticator = OAuth1Authenticator.ForProtectedResource(
                        consumerKey,
                        consumerSecret,
                        string.Empty,  // No token needed for WooCommerce
                        string.Empty   // No token secret needed for WooCommerce
                    )
                };

                _client = new RestClient(options);

                _client.AddDefaultHeader("Accept", "application/json");
            }
            catch (Exception ex)
            {
                throw new WebInvalidFieldException(ex.Source, ex.Message);
            }
        }
        private Task<T> SendRequest<T>(RestRequest request, object body = null, List<ExcludedFields> excludedFields = null)
        {
            try
            {
                // If there is a body, serialize it to JSON and process exclusions
                if (body != null)
                {
                    // Serialize the body to JSON
                    var jsonBody = JsonConvert.SerializeObject(body);

                    // Remove excluded fields from the JSON
                    if (excludedFields != null && excludedFields.Count > 0)
                    {
                        var jsonObject = JObject.Parse(jsonBody);

                        foreach (var excludedField in excludedFields)
                        {
                            var fieldNames = GetFieldNamesFromEnum(excludedField);
                            foreach (var fieldName in fieldNames)
                            {
                                if (jsonObject.ContainsKey(fieldName))
                                {
                                    jsonObject.Remove(fieldName);
                                }
                            }
                        }

                        // Convert the modified JObject back to a string
                        jsonBody = jsonObject.ToString();
                    }

                    // Add the (potentially modified) JSON body to the request
                    request.AddJsonBody(jsonBody);
                }

                // Execute the request
                var response = _client.Execute(request);

                // Decode the response content
                var decodedContent = response.RawBytes != null
                    ? Encoding.UTF8.GetString(response.RawBytes) // Replace UTF8 if needed
                    : response.Content ?? string.Empty;

                // Handle HTTP response codes
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:

                    case HttpStatusCode.Created: // Handle created status for POST requests
                        return Task.FromResult(JsonConvert.DeserializeObject<T>(decodedContent));

                    case HttpStatusCode.NotFound:
                        throw new WebDoesNotExistException();

                    case HttpStatusCode.BadRequest:
                        if (response.Content != null)
                        {
                            if (response.Content.Contains("product_invalid_id"))
                            {
                                throw new WebDoesNotExistException();
                            }
                            if (response.Content.Contains("product_invalid_sku"))
                            {
                                throw new WebInvalidFieldException(WebExceptionFields.InvalidSku, response.Content);
                            }
                            if (response.Content.Contains("stock_quantity")) 
                            {
                                throw new WebInvalidFieldException(WebExceptionFields.InvalidQuantity, response.Content);
                            }
                            if (response.Content.Contains("term_exists"))
                            {
                                throw new WebInvalidFieldException(WebExceptionFields.DuplicateCategoryName, response.Content);
                            }
                        }
                        throw new WebInvalidFieldException("BadRequest", response.Content);

                    case HttpStatusCode.Unauthorized:
                        throw new WebAuthenticationException();

                    case HttpStatusCode.Forbidden:
                        throw new WebInvalidFieldException("Forbidden", response.Content);

                    case HttpStatusCode.InternalServerError:
                        if (response.Content != null)
                        {
                            if (response.Content.Contains("duplicate_term_slug"))
                            {
                                throw new WebInvalidFieldException(WebExceptionFields.DuplicateCategoryName, response.Content);
                            }
                            if (response.Content.Contains("missing_parent"))
                            {
                                throw new WebInvalidFieldException(WebExceptionFields.MissingParentCategoryId, response.Content);
                            }
                        }
                        throw new InternalServerErrorException();

                    case 0:
                        throw new NetworkError();

                    default:
                        throw new WebInvalidFieldException($"Error! Status code: {response.StatusCode}", response.Content);
                }
            }
            catch (WebInvalidFieldException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HTTP Request error: {ex.Message}");
                throw;
            }
        }

        // Map the enum to a list of field names (must match JSON keys)
        private List<string> GetFieldNamesFromEnum(ExcludedFields field)
        {
            switch (field)
            {
                // WebProduct fields
                case ExcludedFields.ProductPrice:
                    return new List<string> { "regular_price", "sale_price" };

                case ExcludedFields.ProductDiscount:
                    return new List<string> { "sale_price" };

                case ExcludedFields.ProductName:
                    return new List<string> { "name" };

                case ExcludedFields.Sku:
                    return new List<string> { "sku" };

                case ExcludedFields.Stock:
                    return new List<string> { "stock_quantity", "manage_stock", "stock_status" };

                case ExcludedFields.ProductAttributes:
                    return new List<string> { "attributes" };

                case ExcludedFields.ProductDescription:
                    return new List<string> { "description", "short_description" };

                case ExcludedFields.CategoryOfProduct:
                    return new List<string> { "categories" };

                case ExcludedFields.DraftStatus:
                    return new List<string> { "status" };

                // WebCategory fields
                case ExcludedFields.CategoryName:
                    return new List<string> { "name" };

                default:
                    throw new ArgumentException($"Unsupported field: {field}");
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
            request.AddOrUpdateParameter("per_page", _pageSize.ToString());
            request.AddOrUpdateParameter("page", _currentPage.ToString());
        }

        /// <summary>
        /// Advances to the next page in the paginated results.
        /// </summary>
        private void NextPage(RestRequest request)
        {
            _currentPage++;
            request.AddOrUpdateParameter("per_page", _pageSize.ToString());
            request.AddOrUpdateParameter("page", _currentPage.ToString());
        }

        public new bool ValidateConnection()
        {
            const string endPoint = "products";
            var request = new RestRequest(endPoint, Method.Get);
            var response = _client.Execute(request);
            return response.StatusCode == HttpStatusCode.OK;
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

        public new int CreateProduct(WebProduct entity, List<ExcludedFields> excludedFields = null)
        {
            const string endpoint = "products";
            var wooProduct = WooCommerceConverters.ToWooProduct(entity);
            var request = new RestRequest(endpoint, Method.Post);
            var createdProduct = SendRequest<WooProduct>(request, wooProduct, excludedFields).Result;
            return WooCommerceConverters.TryToInt(createdProduct.Id);
        }
        public new int UpdateProduct(int id, WebProduct entity, List<ExcludedFields> excludedFields = null)
        {
            if (excludedFields == null)
            {
                excludedFields = new List<ExcludedFields>();
            }
            if (!excludedFields.Contains(ExcludedFields.ProductAttributes))
            {
                excludedFields.Add(ExcludedFields.ProductAttributes);
            }
            if (!excludedFields.Contains(ExcludedFields.DraftStatus))
            {
                excludedFields.Add(ExcludedFields.DraftStatus);
            }
            var endpoint = $"products/{id}";
            var request = new RestRequest(endpoint, Method.Put);
            var updatedProductData = WooCommerceConverters.ToWooProduct(entity);
            var updatedProduct = SendRequest<WooProduct>(request, updatedProductData, excludedFields);

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
        public new IEnumerable<WebProduct> SearchProducts(string searchTerm, ProductTypes productType, int page = 1, int pageSize = 10, int maxPage = 1)
        {
            Utilities.Constants.ProductTypes.TryGetValue(productType, out string productTypeString);
            if (productTypeString == null) 
            {
                return new List<WebProduct>();
            }

            _currentPage = page < 1 ? 1 : page;
            _pageSize = pageSize > 100 || pageSize < 1 ? 100 : pageSize;
            _maxPage = maxPage < 1 ? 1 : maxPage;
            const string endPoint = "products";
            var request = new RestRequest(endPoint, Method.Get);
            if (searchTerm == null)
            {
                return new List<WebProduct>();
            }


            request.AddOrUpdateParameter("per_page", _pageSize.ToString());
            request.AddOrUpdateParameter("page", _currentPage.ToString());

            request.AddParameter("search", searchTerm);
            request.AddParameter("type", productTypeString);

            var results = SendRequest<IList<WooProduct>>(request).Result;
            return results.Select(WooCommerceConverters.ToWebProduct).ToList();
        }
        public new IEnumerable<WebProduct> GetAllProductsWithFields(ProductTypes productType)
        {
            Utilities.Constants.ProductTypes.TryGetValue(productType, out string productTypeString);
            if (productTypeString == null)
            {
                return new List<WebProduct>();
            }

            var fields = new ProductImportImplementation();
            // Create a list to hold field values
            List<string> fieldList = fields.GetType()
                .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(prop => prop.PropertyType == typeof(string))
                .Select(prop => prop.GetValue(fields)?.ToString())
                .Where(value => !string.IsNullOrEmpty(value))
                .ToList();

            const string endPoint = "products";
            var fieldsString = string.Join(",", fieldList);
            var request = new RestRequest(endPoint, Method.Get);
            request.AddParameter("_fields", fieldsString);
            request.AddParameter("type", productTypeString);
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
        public new IEnumerable<WebProduct> GetVariableProductsBySearch(string searchTerm)
        {
            const string endPoint = "products";
            var request = new RestRequest(endPoint, Method.Get);
            request.AddParameter("type", "variable");
            if (searchTerm != null)
            {
                request.AddParameter("search", searchTerm);
            }

            var results = new List<WooProduct>();
            return GetAllWithPagination<WooProduct>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Select(WooCommerceConverters.ToWebProduct).ToList();
        }
        public new IEnumerable<WebProduct> GetVariationProductsByVariableId(int variableId)
        {
            string endPoint = $"products/{variableId}/variations";
            var request = new RestRequest(endPoint, Method.Get);
            var results = new List<WooProduct>();
            return GetAllWithPagination<WooProduct>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Select(WooCommerceConverters.ToWebProduct).ToList();
        }
        public new void UpdateVariationProduct(int variableId, WebProduct variationProduct, List<ExcludedFields> excludedFields = null)
        {
            if (excludedFields == null)
            {
                excludedFields = new List<ExcludedFields>();
            }
            if (!excludedFields.Contains(ExcludedFields.ProductName))
            {
                excludedFields.Add(ExcludedFields.ProductName);
            }
            if (!excludedFields.Contains(ExcludedFields.ProductAttributes))
            {
                excludedFields.Add(ExcludedFields.ProductAttributes);
            }
            if (!excludedFields.Contains(ExcludedFields.DraftStatus))
            {
                excludedFields.Add(ExcludedFields.DraftStatus);
            }
            var endPoint = $"products/{variableId}/variations/{variationProduct.Id}";
            var request = new RestRequest(endPoint, Method.Put);
            var updatedProductData = WooCommerceConverters.ToWooProduct(variationProduct);
            SendRequest<WooProduct>(request, updatedProductData, excludedFields);
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
        public new int CreateCategory(WebCategory entity, List<ExcludedFields> excludedFields = null)
        {
            const string endPoint = "products/categories";
            var wooCategory = WooCommerceConverters.ToWooCategory(entity);
            var request = new RestRequest(endPoint, Method.Post);
            var createdCategory = SendRequest<WooCategory>(request, wooCategory, excludedFields).Result;
            return WooCommerceConverters.TryToInt(createdCategory.Id);
        }

        public new int UpdateCategory(int id, WebCategory entity, List<ExcludedFields> excludedFields = null)
        {
            var endPoint = $"products/categories/{id}";
            var request = new RestRequest(endPoint, Method.Put);
            var updatedCategoryData = WooCommerceConverters.ToWooCategory(entity);
            var updatedCategory = SendRequest<WooCategory>(request, updatedCategoryData, excludedFields);
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
        
        #region Customer
        
        public new IEnumerable<WebCustomer> SearchCustomers(string searchTerm, int page = 1, int pageSize = 10, int maxPage = 1)
        {   
            _currentPage = page < 1 ? 1 : page;
            _pageSize = pageSize > 100 || pageSize < 1 ? 100 : pageSize;
            _maxPage = maxPage < 1 ? 1 : maxPage;
            const string endPoint = "customers";
            var request = new RestRequest(endPoint, Method.Get);
            if (searchTerm == null)
            {
                return new List<WebCustomer>();
            }
            
            request.AddParameter("search", searchTerm);
            var results = new List<WooCustomer>();
            return GetAllWithPagination<WooCustomer>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Select(WooCommerceConverters.ToWebCustomer).ToList();
        }
        
        public new IEnumerable<WebCustomer> GetAllCustomersWithFields(IList<string> fields)
        {
            if (fields == null || !fields.Any())
            {
                return new List<WebCustomer>();
            }
            const string endPoint = "customers";
            var fieldsString = string.Join(",", fields);
            var request = new RestRequest(endPoint, Method.Get);
            request.AddParameter("_fields", fieldsString);
            var results = new List<WooCustomer>();
            return GetAllWithPagination<WooCustomer>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Select(WooCommerceConverters.ToWebCustomer).ToList();
        }

        public new IEnumerable<KeyValuePair<int, string>> GetCustomerIdAndNameBySearch(
            string searchTerm,
            int page = 1,
            int pageSize = 10,
            int maxPage = 1)
        {
            var fields = new List<string> { "id", "first_name", "last_name" };
            _currentPage = page < 1 ? 1 : page;
            _currentPage = page < 1 ? 1 : page;
            _pageSize = pageSize > 100 || pageSize < 1 ? 100 : pageSize;
            _maxPage = maxPage < 1 ? 1 : maxPage;
            const string endPoint = "customers";
            var request = new RestRequest(endPoint, Method.Get);
            if (searchTerm == null)
            {
                return new List<KeyValuePair<int, string>>();
            }
            
            request.AddParameter("search", searchTerm);
            request.AddParameter("_fields", string.Join(",", fields));
            var results = new List<WooCustomer>();
            return GetAllWithPagination<WooCustomer>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Select(w =>
                new KeyValuePair<int, string>(w.Id, w.FirstName + " " +w.LastName))
                .ToList();
        }
        public new WebCustomer GetCustomerById(int id)
        {
            var endPoint = $"customers/{id}";
            var request = new RestRequest(endPoint, Method.Get);
            var wooCustomer = SendRequest<WooCustomer>(request).Result;
            return WooCommerceConverters.ToWebCustomer(wooCustomer);
        }
        #endregion

        #region Orders

        public new IEnumerable<WebOrder> GetAllOrdersExcludeById(IEnumerable<int> idsToExclude, DateTime? startDate, DateTime? endDate)
        {
            const string endPoint = "orders";
            var idsToExcludeString = string.Join(",", idsToExclude);
            var request = new RestRequest(endPoint, Method.Get);
            request.AddParameter("exclude", idsToExcludeString);

            if (startDate.HasValue)
            {
                request.AddParameter("after", startDate.Value.ToString("o")); // ISO 8601 format
            }
            if (endDate.HasValue)
            {
                request.AddParameter("before", endDate.Value.ToString("o")); // ISO 8601 format
            }
            var results = new List<WooOrder>();
            return GetAllWithPagination<WooOrder>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Select(WooCommerceConverters.ToWebOrder).ToList();
        }
        public new IEnumerable<WebOrder> GetOrdersByFilters(DateTime? startDate, DateTime? endDate, IEnumerable<int> idsToExclude = null, IEnumerable<OrderStatus> orderStatuses = null)
        {
            const string endPoint = "orders";
            if (idsToExclude == null)
            {
                idsToExclude = new List<int>();
            }
            var idsToExcludeString = string.Join(",", idsToExclude);
            var request = new RestRequest(endPoint, Method.Get);
            request.AddParameter("exclude", idsToExcludeString);
            if (orderStatuses != null && orderStatuses.Any())
            {
                if (!orderStatuses.Contains(OrderStatus.Other))
                {
                    string orderStatusString = string.Join(",", orderStatuses.Select(orderStatus => Constants.OrderStatuses[orderStatus]));
                    request.AddParameter("status", orderStatusString);
                }
            }
            else
            {
                return new List<WebOrder>();
            }
            if (startDate.HasValue)
            {
                request.AddParameter("after", startDate.Value.ToString("o")); // ISO 8601 format
            }
            if (endDate.HasValue)
            {
                request.AddParameter("before", endDate.Value.ToString("o")); // ISO 8601 format
            }
            var results = new List<WooOrder>();
            return GetAllWithPagination<WooOrder>(request, pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            }).Select(WooCommerceConverters.ToWebOrder).ToList();
        }

        public new IEnumerable<WebOrder> GetOrdersBySearch(
            IEnumerable<int> idsToExclude,
            string searchTerm,
            IEnumerable<OrderStatus> orderStatuses,
            int? customerId,
            decimal totalMin,
            decimal totalMax,
            DateTime startDate,
            DateTime endDate, 
            int page = 1,
            int pageSize = 10,
            int maxPage = 1)
        {
            _currentPage = page < 1 ? 1 : page;
            _pageSize = pageSize > 100 || pageSize < 1 ? 100 : pageSize;
            _maxPage = maxPage < 1 ? 1 : maxPage;
            const string endPoint = "orders";
            var request = new RestRequest(endPoint, Method.Get);

            request.AddOrUpdateParameter("per_page", _pageSize.ToString());
            request.AddOrUpdateParameter("page", _currentPage.ToString());

            if (orderStatuses != null && orderStatuses.Any())
            {
                if (!orderStatuses.Contains(OrderStatus.Other))
                {
                    string orderStatusString = string.Join(",", orderStatuses.Select(orderStatus => Constants.OrderStatuses[orderStatus]));
                    request.AddParameter("status", orderStatusString);
                }
            }
            else
            {
                return new List<WebOrder>();
            }
            if (idsToExclude != null && idsToExclude.Any())
            {
                string idsToExcludeString = string.Join(",", idsToExclude);
                request.AddParameter("exclude", idsToExcludeString);
            }
            if (searchTerm != null)
            {
                request.AddParameter("search", searchTerm);
            }

            if (customerId > 0)
            {
                request.AddParameter("customer", customerId.Value);
            }
            if (totalMin > 0)
            {
                request.AddParameter("total_min", totalMin);
            }
            if (totalMax > 0)
            {
                request.AddParameter("total_max", totalMax);
            }
            if (startDate != DateTime.MinValue)
            {
                request.AddParameter("after", startDate.ToString("o")); // ISO 8601 format
            }
            if (endDate != DateTime.MinValue)
            {
                request.AddParameter("before", endDate.ToString("o")); // ISO 8601 format
            }
            var results = SendRequest<IList<WooOrder>>(request).Result;
            return results.Select(WooCommerceConverters.ToWebOrder).ToList();
        }
        
        public new void UpdateOrderStatus(int orderId, OrderStatus orderStatus)
        {
            string endPoint = $"orders/{orderId}";
            var request = new RestRequest(endPoint, Method.Put);
            string status = Constants.OrderStatuses[orderStatus];
            request.AddParameter("status", status);
            SendRequest<WooOrder>(request);
        }
        #endregion

        #region PaymentMethods

        public new IEnumerable<WebPaymentMethod> GetAllPaymentMethods()
        {
            const string endPoint = "payment_gateways";
            var request = new RestRequest(endPoint, Method.Get);
            var paymentMethods = SendRequest<IList<WooPaymentMethod>>(request).Result;
            return paymentMethods.Select(WooCommerceConverters.ToWebPaymentMethod);
        }

        #endregion
        
        public new void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
