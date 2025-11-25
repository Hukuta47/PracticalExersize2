using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEBKAMAC.Models;
using WEBKAMAC.Services;

namespace WEBKAMAC.Pages
{
    public class CalculateRawMaterialModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<CalculateRawMaterialModel> _logger;

        public CalculateRawMaterialModel(IApiService apiService, ILogger<CalculateRawMaterialModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        [BindProperty]
        public int ProductTypeID { get; set; }

        [BindProperty]
        public int MaterialTypeID { get; set; }

        [BindProperty]
        public int Quantity { get; set; } = 1;

        [BindProperty]
        public double Param1 { get; set; } = 1.0;

        [BindProperty]
        public double Param2 { get; set; } = 1.0;

        public List<SelectListItem> ProductTypes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MaterialTypes { get; set; } = new List<SelectListItem>();

        public int? Result { get; set; }
        public string Message { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;

        public string? ProductTypeName { get; set; }
        public string? MaterialTypeName { get; set; }

        public async Task OnGetAsync()
        {
            await LoadReferenceData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Quantity <= 0 || Param1 <= 0 || Param2 <= 0)
                {
                    Message = "Количество и параметры должны быть положительными числами";
                    MessageType = "error";
                    await LoadReferenceData();
                    return Page();
                }

                Result = await _apiService.CalculateRawMaterialAsync(
                    ProductTypeID, MaterialTypeID, Quantity, Param1, Param2);

                if (Result == -1)
                {
                    Message = "Ошибка расчета. Проверьте введенные данные";
                    MessageType = "error";
                }
                else
                {
                    // Получаем названия для отображения
                    var productTypes = await _apiService.GetProductTypesAsync();
                    var materialTypes = await _apiService.GetMaterialTypesAsync();

                    ProductTypeName = productTypes.FirstOrDefault(pt => pt.ProductTypeID == ProductTypeID)?.ProductTypeName;
                    MaterialTypeName = materialTypes.FirstOrDefault(mt => mt.MaterialTypeID == MaterialTypeID)?.MaterialTypeName;

                    Message = "Расчет выполнен успешно";
                    MessageType = "success";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating raw material");
                Message = "Произошла ошибка при расчете";
                MessageType = "error";
            }

            await LoadReferenceData();
            return Page();
        }

        private async Task LoadReferenceData()
        {
            try
            {
                var productTypes = await _apiService.GetProductTypesAsync();
                ProductTypes = productTypes.Select(pt => new SelectListItem
                {
                    Value = pt.ProductTypeID.ToString(),
                    Text = $"{pt.ProductTypeName} (коэф: {pt.ProductTypeCoefficient})"
                }).ToList();

                var materialTypes = await _apiService.GetMaterialTypesAsync();
                MaterialTypes = materialTypes.Select(mt => new SelectListItem
                {
                    Value = mt.MaterialTypeID.ToString(),
                    Text = $"{mt.MaterialTypeName} (потери: {mt.RawMaterialLossPercent}%)"
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading reference data for calculation");
            }
        }
    }
}