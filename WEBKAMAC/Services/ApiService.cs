using System.Text;
using System.Text.Json;
using WEBKAMAC.Models;

namespace WEBKAMAC.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        // Products
        public async Task<List<Product>> GetProductsAsync()
        {
            return await GetAsync<List<Product>>("api/products");
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            return await GetAsync<Product>($"api/products/{id}");
        }

        public async Task<bool> CreateProductAsync(ProductCreateEditModel product)
        {
            try
            {
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/products", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Product created successfully");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to create product. Status: {response.StatusCode}, Error: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return false;
            }
        }

        public async Task<bool> UpdateProductAsync(int id, ProductCreateEditModel product)
        {
            try
            {
                var json = JsonSerializer.Serialize(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/products/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Product {id} updated successfully");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to update product {id}. Status: {response.StatusCode}, Error: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product {id}");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/products/{id}");

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Product {id} deleted successfully");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to delete product {id}. Status: {response.StatusCode}, Error: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product {id}");
                return false;
            }
        }

        // Workshops
        public async Task<List<Workshop>> GetWorkshopsAsync()
        {
            return await GetAsync<List<Workshop>>("api/workshops");
        }

        public async Task<List<ProductManufacturing>> GetProductManufacturingAsync(int productId)
        {
            return await GetAsync<List<ProductManufacturing>>($"api/manufacturing/product/{productId}");
        }

        // Reference data
        public async Task<List<ProductType>> GetProductTypesAsync()
        {
            return await GetAsync<List<ProductType>>("api/reference/producttypes");
        }

        public async Task<List<MaterialType>> GetMaterialTypesAsync()
        {
            return await GetAsync<List<MaterialType>>("api/reference/materialtypes");
        }

        public async Task<List<WorkshopType>> GetWorkshopTypesAsync()
        {
            return await GetAsync<List<WorkshopType>>("api/reference/workshoptypes");
        }

        // Analysis
        public async Task<List<ManufacturingData>> GetManufacturingDataAsync()
        {
            return await GetAsync<List<ManufacturingData>>("api/manufacturing");
        }

        public async Task<List<ProductionSummary>> GetProductionSummaryAsync()
        {
            return await GetAsync<List<ProductionSummary>>("api/analysis/summary");
        }

        public async Task<List<ProductTotalTime>> GetProductsTotalTimeAsync()
        {
            return await GetAsync<List<ProductTotalTime>>("api/analysis/products/totaltime");
        }

        // Calculation
        public async Task<int> CalculateRawMaterialAsync(int productTypeId, int materialTypeId, int quantity, double param1, double param2)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"api/calculation/rawmaterial?" +
                    $"productTypeId={productTypeId}&" +
                    $"materialTypeId={materialTypeId}&" +
                    $"quantity={quantity}&" +
                    $"param1={param1}&" +
                    $"param2={param2}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<int>(content, _jsonOptions);
                }
                else
                {
                    _logger.LogError($"Calculation failed: {response.StatusCode}");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating raw material");
                return -1;
            }
        }

        // Private helper methods
        private async Task<T> GetAsync<T>(string endpoint) where T : new()
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(content, _jsonOptions) ?? new T();
                }
                else
                {
                    _logger.LogWarning($"API call failed: {endpoint}, Status: {response.StatusCode}");
                    return new T();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error calling {endpoint}");
                return new T();
            }
        }
    }
}