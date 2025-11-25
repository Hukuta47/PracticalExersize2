using Microsoft.AspNetCore.Mvc.RazorPages;
using WEBKAMAC.Models;
using WEBKAMAC.Services;

namespace WEBKAMAC.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IApiService apiService, ILogger<IndexModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public List<Product> Products { get; set; } = new List<Product>();
        public List<ManufacturingData> ManufacturingData { get; set; } = new List<ManufacturingData>();
        public List<ProductionSummary> ProductionSummary { get; set; } = new List<ProductionSummary>();

        public int TotalProducts => Products.Count;
        public int TotalWorkshops => ManufacturingData.Select(m => m.WorkshopName).Distinct().Count();
        public decimal TotalManufacturingTime => ManufacturingData.Sum(m => m.ManufacturingTimeHours);

        // Добавляем свойство HasData
        public bool HasData => Products.Any() || ManufacturingData.Any();

        public async Task OnGetAsync()
        {
            try
            {
                _logger.LogInformation("Loading data from API...");

                // Load data in parallel for better performance
                var productsTask = _apiService.GetProductsAsync();
                var manufacturingTask = _apiService.GetManufacturingDataAsync();
                var summaryTask = _apiService.GetProductionSummaryAsync();

                await Task.WhenAll(productsTask, manufacturingTask, summaryTask);

                Products = await productsTask;
                ManufacturingData = await manufacturingTask;
                ProductionSummary = await summaryTask;

                _logger.LogInformation($"Loaded {Products.Count} products, {ManufacturingData.Count} manufacturing records");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading data from API");
                // Data will be empty, but page will still load
            }
        }
    }
}