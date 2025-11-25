using ComfortDesktop.Models;
using ComfortDesktop.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComfortDesktop.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private ObservableCollection<Product> _products;
        private ObservableCollection<ProductType> _productTypes;
        private ObservableCollection<MaterialType> _materialTypes;
        private ProductCreateEditModel _newProduct;
        private Product _selectedProduct;

        public ProductsViewModel(IApiService apiService)
        {
            _apiService = apiService;
            _products = new ObservableCollection<Product>();
            _productTypes = new ObservableCollection<ProductType>();
            _materialTypes = new ObservableCollection<MaterialType>();
            _newProduct = new ProductCreateEditModel();

            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
            CreateProductCommand = new RelayCommand(async () => await CreateProductAsync());
            UpdateProductCommand = new RelayCommand(async () => await UpdateProductAsync());
            DeleteProductCommand = new RelayCommand(async () => await DeleteProductAsync());

            _ = LoadDataAsync();
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public ObservableCollection<ProductType> ProductTypes
        {
            get => _productTypes;
            set => SetProperty(ref _productTypes, value);
        }

        public ObservableCollection<MaterialType> MaterialTypes
        {
            get => _materialTypes;
            set => SetProperty(ref _materialTypes, value);
        }

        public ProductCreateEditModel NewProduct
        {
            get => _newProduct;
            set => SetProperty(ref _newProduct, value);
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                if (value != null)
                {
                    NewProduct = new ProductCreateEditModel
                    {
                        ArticleNumber = value.ArticleNumber,
                        ProductName = value.ProductName,
                        ProductTypeID = value.ProductTypeID,
                        MaterialTypeID = value.MaterialTypeID,
                        MinPartnerPrice = value.MinPartnerPrice
                    };
                }
            }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand CreateProductCommand { get; }
        public ICommand UpdateProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        private async Task LoadDataAsync()
        {
            try
            {
                var products = await _apiService.GetProductsAsync();
                var productTypes = await _apiService.GetProductTypesAsync();
                var materialTypes = await _apiService.GetMaterialTypesAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Products.Clear();
                    foreach (var product in products)
                        Products.Add(product);

                    ProductTypes.Clear();
                    foreach (var type in productTypes)
                        ProductTypes.Add(type);

                    MaterialTypes.Clear();
                    foreach (var material in materialTypes)
                        MaterialTypes.Add(material);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CreateProductAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewProduct.ArticleNumber) ||
                    string.IsNullOrWhiteSpace(NewProduct.ProductName) ||
                    NewProduct.MinPartnerPrice <= 0)
                {
                    MessageBox.Show("Заполните все обязательные поля", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var success = await _apiService.CreateProductAsync(NewProduct);
                if (success)
                {
                    MessageBox.Show("Продукт успешно создан", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    NewProduct = new ProductCreateEditModel();
                    await LoadDataAsync();
                }
                else
                {
                    MessageBox.Show("Ошибка при создании продукта", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateProductAsync()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Выберите продукт для редактирования", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var success = await _apiService.UpdateProductAsync(SelectedProduct.ProductID, NewProduct);
                if (success)
                {
                    MessageBox.Show("Продукт успешно обновлен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadDataAsync();
                }
                else
                {
                    MessageBox.Show("Ошибка при обновлении продукта", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteProductAsync()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Выберите продукт для удаления", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить продукт '{SelectedProduct.ProductName}'?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var success = await _apiService.DeleteProductAsync(SelectedProduct.ProductID);
                    if (success)
                    {
                        MessageBox.Show("Продукт успешно удален", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadDataAsync();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении продукта", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}