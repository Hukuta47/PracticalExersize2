using ComfortDesktop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComfortDesktop.Services
{
    public interface IApiService
    {
        // Products
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(int id);
        Task<bool> CreateProductAsync(ProductCreateEditModel product);
        Task<bool> UpdateProductAsync(int id, ProductCreateEditModel product);
        Task<bool> DeleteProductAsync(int id);

        // Workshops
        Task<List<Workshop>> GetWorkshopsAsync();
        Task<List<ProductManufacturing>> GetProductManufacturingAsync(int productId);

        // Reference data
        Task<List<ProductType>> GetProductTypesAsync();
        Task<List<MaterialType>> GetMaterialTypesAsync();
        Task<List<WorkshopType>> GetWorkshopTypesAsync();

        // Analysis
        Task<List<ManufacturingData>> GetManufacturingDataAsync();
        Task<List<ProductionSummary>> GetProductionSummaryAsync();
        Task<List<ProductTotalTime>> GetProductsTotalTimeAsync();

        // Calculation
        Task<int> CalculateRawMaterialAsync(int productTypeId, int materialTypeId, int quantity, double param1, double param2);
    }
}