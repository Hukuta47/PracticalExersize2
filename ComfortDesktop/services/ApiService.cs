using ComfortDesktop.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ComfortDesktop.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44366/")
            };
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        // Products
        public async Task<List<Product>> GetProductsAsync()
        {
            return await GetAsync<List<Product>>("api/products");
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await GetAsync<Product>($"api/products/{id}");
        }

        public async Task<bool> CreateProductAsync(ProductCreateEditModel product)
        {
            return await PostAsync("api/products", product);
        }

        public async Task<bool> UpdateProductAsync(int id, ProductCreateEditModel product)
        {
            return await PutAsync($"api/products/{id}", product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await DeleteAsync($"api/products/{id}");
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
                    return JsonConvert.DeserializeObject<int>(content);
                }
                return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        // Private helper methods - исправленные для C# 7.3
        private async Task<T> GetAsync<T>(string endpoint) where T : new()
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<T>(content);
                    return result != null ? result : new T();
                }
                return new T();
            }
            catch (Exception)
            {
                return new T();
            }
        }

        private async Task<bool> PostAsync<T>(string endpoint, T data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endpoint, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> PutAsync<T>(string endpoint, T data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(endpoint, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}