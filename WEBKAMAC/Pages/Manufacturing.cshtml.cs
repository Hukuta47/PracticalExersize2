using Microsoft.AspNetCore.Mvc.RazorPages;
using WEBKAMAC.Models;
using WEBKAMAC.Services;

namespace WEBKAMAC.Pages
{
    public class ManufacturingModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ManufacturingModel> _logger;

        public ManufacturingModel(IApiService apiService, ILogger<ManufacturingModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public List<ManufacturingData> ManufacturingData { get; set; } = new List<ManufacturingData>();

        public async Task OnGetAsync()
        {
            try
            {
                ManufacturingData = await _apiService.GetManufacturingDataAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading manufacturing data");
            }
        }
    }
}