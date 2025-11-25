using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEBKAMAC.Models;
using WEBKAMAC.Services;

namespace WEBKAMAC.Pages
{
    public class EditProductModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<EditProductModel> _logger;

        public EditProductModel(IApiService apiService, ILogger<EditProductModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        [BindProperty]
        public ProductCreateEditModel Product { get; set; } = new ProductCreateEditModel();

        public List<SelectListItem> ProductTypes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MaterialTypes { get; set; } = new List<SelectListItem>();

        public string Message { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var product = await _apiService.GetProductAsync(id);
                if (product == null)
                {
                    return RedirectToPage("/Products");
                }

                Product = new ProductCreateEditModel
                {
                    ArticleNumber = product.ArticleNumber,
                    ProductName = product.ProductName,
                    ProductTypeID = product.ProductTypeID,
                    MaterialTypeID = product.MaterialTypeID,
                    MinPartnerPrice = product.MinPartnerPrice
                };

                await LoadReferenceData();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading product for edit");
                return RedirectToPage("/Products");
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadReferenceData();
                    Message = "Пожалуйста, исправьте ошибки в форме";
                    MessageType = "error";
                    return Page();
                }

                if (Product.MinPartnerPrice < 0)
                {
                    await LoadReferenceData();
                    Message = "Стоимость не может быть отрицательной";
                    MessageType = "error";
                    return Page();
                }

                // Реальный вызов API
                var success = await _apiService.UpdateProductAsync(id, Product);

                if (success)
                {
                    TempData["Message"] = "Продукт успешно обновлен";
                    TempData["MessageType"] = "success";
                    return RedirectToPage("/Products");
                }
                else
                {
                    Message = "Ошибка при обновлении продукта";
                    MessageType = "error";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product");
                Message = "Произошла ошибка при обновлении продукта";
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
                    Text = pt.ProductTypeName
                }).ToList();

                var materialTypes = await _apiService.GetMaterialTypesAsync();
                MaterialTypes = materialTypes.Select(mt => new SelectListItem
                {
                    Value = mt.MaterialTypeID.ToString(),
                    Text = mt.MaterialTypeName
                }).ToList();

                _logger.LogInformation($"Loaded {ProductTypes.Count} product types and {MaterialTypes.Count} material types for edit");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading reference data for edit");
            }
        }
    }
}