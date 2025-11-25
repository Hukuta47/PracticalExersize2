using Microsoft.AspNetCore.Mvc.RazorPages;
using WEBKAMAC.Models;
using WEBKAMAC.Services;

namespace WEBKAMAC.Pages
{
    public class AnalysisModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<AnalysisModel> _logger;

        public AnalysisModel(IApiService apiService, ILogger<AnalysisModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public List<ProductionSummary> ProductionSummary { get; set; } = new List<ProductionSummary>();
        public List<ProductTotalTime> ProductsTotalTime { get; set; } = new List<ProductTotalTime>();

        public async Task OnGetAsync()
        {
            try
            {
                ProductionSummary = await _apiService.GetProductionSummaryAsync();
                ProductsTotalTime = await _apiService.GetProductsTotalTimeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading analysis data");
            }
        }
    }
}