using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Repositories;
using WooCommerceApi.Helpers;

namespace WooCommerceApi.Repositories
{
    /// <summary>
    /// Abstract base class for WooCommerce repository, using RestSharp and DI.
    /// Handles API interactions for common CRUD operations (Create, Read, Update, Delete).
    /// </summary>
    /// <typeparam name="T">The type of the entity being managed (e.g., products, orders).</typeparam>
    public abstract class WebRepository<T> : IWebRepository<T> where T : class
    {
        protected readonly IRestClient _client;
        protected readonly string _baseApiUrl;
        protected string _endpoint;
        private int _currentPage = 1;
        private readonly Dictionary<string, string> _apiConfigs;
        private readonly QueryFilterBuilder _queryBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebRepository{T}"/> class.
        /// Injects RestClient via DI and configures authentication headers.
        /// </summary>
        /// <param name="apiConfigs">API configuration including base URL, consumer key, and secret.</param>
        /// <param name="endpoint">The specific endpoint for the entity (e.g., "products", "orders").</param>
        public WebRepository(Dictionary<string, string> apiConfigs, string endpoint)
        {
            _apiConfigs = apiConfigs;
            _client = new RestClient(baseUrl: _apiConfigs["BaseUrl"]);
            _queryBuilder = new QueryFilterBuilder();
            SetParameters(endpoint);

            // Set default headers
            _client.AddDefaultHeader("Accept", "application/json");

            // Set authentication using WooCommerce API keys
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_apiConfigs["ConsumerKey"]}:{_apiConfigs["ConsumerSecret"]}"));
            _client.AddDefaultHeader("Authorization", $"Basic {credentials}");
        }

        /// <summary>
        /// Sets the endpoint with pagination and any additional filters using the QueryFilterBuilder.
        /// </summary>
        /// <param name="endpoint">The specific endpoint for the API call.</param>
        protected void SetParameters(string endpoint)
        {
            _queryBuilder
                .AddOrUpdateParameter("page", _currentPage.ToString())
                .AddOrUpdateParameter("per_page", "100");

            _endpoint = $"wp-json/wc/v3/{endpoint}";
        }
        protected void ResetPage()
        {
            _currentPage = 1;
            _queryBuilder.AddOrUpdateParameter("page", _currentPage.ToString());
        }

        /// <summary>
        /// Advances to the next page in the paginated results.
        /// </summary>
        protected void NextPage()
        {
            _currentPage++;
            _queryBuilder.AddOrUpdateParameter("page", _currentPage.ToString());
        }

        /// <summary>
        /// Retrieves a single entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity of type <typeparamref name="T"/>.</returns>
        public virtual T GetById(int id)
        {
            var requestUrl = $"{_endpoint}/{id}";
            var request = new RestRequest(requestUrl, Method.Get);
            var response = _client.Execute(request);
            EnsureSuccessStatusCode(response);

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        /// <summary>
        /// Retrieves all entities from the WooCommerce API using paginated requests.
        /// </summary>
        /// <returns>An enumerable of entities of type <typeparamref name="T"/>.</returns>
        public virtual IEnumerable<T> GetAll()
        {
            List<T> results = new List<T>(); // Collection to store the results

            // Use the pagination method, passing in a function to process each page
            GetAllWithPagination(pageResults =>
            {
                // Add the page results to the overall results list
                results.AddRange(pageResults);

                // Always return true to continue fetching all pages
                return true;
            });

            return results; // Return the complete set of results
        }

        /// <summary>
        /// Retrieves all entities with only the specified fields.
        /// </summary>
        /// <param name="fields">List of fields to include in the response.</param>
        /// <returns>An enumerable of entities of type <typeparamref name="T"/>.</returns>
        public virtual IEnumerable<T> GetAllWithFields(IEnumerable<string> fields)
        {
            if (fields == null || !fields.Any())
            {
                throw new ArgumentException("Fields list cannot be empty.");
            }

            var fieldsString = string.Join(",", fields);
            _queryBuilder.AddOrUpdateParameter("_fields", fieldsString);

            List<T> results = new List<T>();

            return GetAllWithPagination(pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            });
        }


        /// <summary>
        /// Retrieves all entities excluding those with specified IDs.
        /// </summary>
        /// <param name="idsToExclude">A collection of IDs to exclude from the results.</param>
        /// <returns>An enumerable of entities of type <typeparamref name="T"/>.</returns>
        public virtual IEnumerable<T> GetAllExcludingIds(IEnumerable<int> idsToExclude)
        {
            var excludeIds = string.Join(",", idsToExclude);
            _queryBuilder.AddOrUpdateParameter("exclude", excludeIds);

            List<T> results = new List<T>();

            return GetAllWithPagination(pageResults =>
            {
                results.AddRange(pageResults);
                return true;
            });
        }

        /// <summary>
        /// Creates a new entity in WooCommerce.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        public virtual void Create(T entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var request = new RestRequest(_endpoint, Method.Post);
            request.AddJsonBody(json);

            var response = _client.Execute(request);
            EnsureSuccessStatusCode(response);
        }

        /// <summary>
        /// Updates an existing entity in WooCommerce.
        /// </summary>
        /// <param name="id">The ID of the entity to update.</param>
        /// <param name="entity">The updated entity data.</param>
        public virtual void Update(int id, T entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var requestUrl = $"{_endpoint}/{id}";
            var request = new RestRequest(requestUrl, Method.Put);
            request.AddJsonBody(json);

            var response = _client.Execute(request);
            EnsureSuccessStatusCode(response);
        }

        /// <summary>
        /// Deletes an entity by its ID in WooCommerce.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>The response content as a string.</returns>
        public virtual string Delete(int id)
        {
            var requestUrl = $"{_endpoint}/{id}";
            var request = new RestRequest(requestUrl, Method.Delete);
            var response = _client.Execute(request);
            EnsureSuccessStatusCode(response);

            return response.Content;
        }

        /// <summary>
        /// Ensures that the HTTP request was successful.
        /// Throws an exception if the response indicates failure.
        /// </summary>
        /// <param name="response">The HTTP response to check.</param>
        private void EnsureSuccessStatusCode(RestResponse response)
        {
            if (!response.IsSuccessful)
            {
                throw new InvalidOperationException($"Error: {response.StatusCode}, Content: {response.Content}");
            }
        }

        /// <summary>
        /// Retrieves all entities using a custom processing function for each page.
        /// </summary>
        /// <param name="processPage">A function to process each page of results. It should return true if you want to continue fetching more pages, false to stop.</param>
        /// <returns>An enumerable of entities of type <typeparamref name="T"/>.</returns>
        public virtual IEnumerable<T> GetAllWithPagination(Func<IEnumerable<T>, bool> processPage)
        {
            List<T> results = new List<T>();
            while (true)
            {
                // Build the request URL for the current page
                var requestUrl = _queryBuilder.Build(_endpoint);
                var request = new RestRequest(requestUrl, Method.Get);

                // Execute the request
                var response = _client.Execute(request);
                EnsureSuccessStatusCode(response);

                // Deserialize the response content into the expected entity type
                var result = JsonConvert.DeserializeObject<IEnumerable<T>>(response.Content);
                if (result == null || !result.Any())
                {
                    // No more results, stop pagination
                    break;
                }

                // Add the results from this page to the overall collection
                results.AddRange(result);

                // Call the processing function for the current page
                // If the function returns false, stop fetching more pages
                if (!processPage(result))
                {
                    break;
                }

                // Move to the next page
                NextPage();
            }
            ResetPage();
            return results;
        }

        #region IDisposable Implementation
        protected virtual void Dispose(bool disposing) { }

        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }

        ~WebRepository()
        {
            Dispose(false);
        }
        #endregion
    }
}
