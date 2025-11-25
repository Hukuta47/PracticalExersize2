using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEBKAMAC.Models;
using WEBKAMAC.Services;

namespace WEBKAMAC.Pages
{
    public class ProductWorkshopsModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ProductWorkshopsModel> _logger;

        public ProductWorkshopsModel(IApiService apiService, ILogger<ProductWorkshopsModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public Product? Product { get; set; }
        public List<ProductManufacturing> ManufacturingData { get; set; } = new List<ProductManufacturing>();
        public int TotalManufacturingTime => (int)ManufacturingData.Sum(m => m.ManufacturingTimeHours);

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Product = await _apiService.GetProductAsync(id);
                if (Product == null)
                {
                    return RedirectToPage("/Products");
                }

                ManufacturingData = await _apiService.GetProductManufacturingAsync(id);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product workshops");
                return RedirectToPage("/Products");
            }
        }
    }
}