using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEBKAMAC.Models;
using WEBKAMAC.Services;

namespace WEBKAMAC.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ProductsModel> _logger;

        public ProductsModel(IApiService apiService, ILogger<ProductsModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public List<Product> Products { get; set; } = new List<Product>();
        public List<SelectListItem> ProductTypes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MaterialTypes { get; set; } = new List<SelectListItem>();

        [BindProperty]
        public ProductCreateEditModel NewProduct { get; set; } = new ProductCreateEditModel();

        public string Message { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadDataAsync();
                    Message = "Пожалуйста, исправьте ошибки в форме";
                    MessageType = "error";
                    return Page();
                }

                if (NewProduct.MinPartnerPrice < 0)
                {
                    await LoadDataAsync();
                    Message = "Стоимость не может быть отрицательной";
                    MessageType = "error";
                    return Page();
                }

                // Реальный вызов API
                var success = await _apiService.CreateProductAsync(NewProduct);

                if (success)
                {
                    Message = "Продукт успешно добавлен";
                    MessageType = "success";
                    NewProduct = new ProductCreateEditModel(); // Clear form

                    // Перезагружаем список продуктов
                    Products = await _apiService.GetProductsAsync();
                }
                else
                {
                    Message = "Ошибка при добавлении продукта";
                    MessageType = "error";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                Message = "Произошла ошибка при добавлении продукта";
                MessageType = "error";
            }

            await LoadDataAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                // Реальный вызов API
                var success = await _apiService.DeleteProductAsync(id);

                if (success)
                {
                    Message = "Продукт успешно удален";
                    MessageType = "success";

                    // Перезагружаем список продуктов
                    Products = await _apiService.GetProductsAsync();
                }
                else
                {
                    Message = "Ошибка при удалении продукта";
                    MessageType = "error";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                Message = "Произошла ошибка при удалении продукта";
                MessageType = "error";
            }

            await LoadDataAsync();
            return Page();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Загружаем продукты
                Products = await _apiService.GetProductsAsync();

                // Загружаем типы продукции
                var productTypes = await _apiService.GetProductTypesAsync();
                ProductTypes = productTypes.Select(pt => new SelectListItem
                {
                    Value = pt.ProductTypeID.ToString(),
                    Text = pt.ProductTypeName
                }).ToList();

                // Загружаем типы материалов
                var materialTypes = await _apiService.GetMaterialTypesAsync();
                MaterialTypes = materialTypes.Select(mt => new SelectListItem
                {
                    Value = mt.MaterialTypeID.ToString(),
                    Text = mt.MaterialTypeName
                }).ToList();

                _logger.LogInformation($"Loaded {ProductTypes.Count} product types and {MaterialTypes.Count} material types");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading data");
                Message = "Ошибка при загрузке данных";
                MessageType = "error";
            }
        }
    }
}