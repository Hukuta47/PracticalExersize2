using ComfortDesktop.Models;
using ComfortDesktop.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ComfortDesktop.ViewModels
{
    public class CalculationViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private ObservableCollection<ProductType> _productTypes;
        private ObservableCollection<MaterialType> _materialTypes;
        private int _selectedProductTypeId;
        private int _selectedMaterialTypeId;
        private int _quantity = 1;
        private double _param1 = 1.0;
        private double _param2 = 1.0;
        private int _result;

        public CalculationViewModel(IApiService apiService)
        {
            _apiService = apiService;
            _productTypes = new ObservableCollection<ProductType>();
            _materialTypes = new ObservableCollection<MaterialType>();

            CalculateCommand = new RelayCommand(async () => await CalculateAsync());
            _ = LoadReferenceDataAsync();
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

        public int SelectedProductTypeId
        {
            get => _selectedProductTypeId;
            set => SetProperty(ref _selectedProductTypeId, value);
        }

        public int SelectedMaterialTypeId
        {
            get => _selectedMaterialTypeId;
            set => SetProperty(ref _selectedMaterialTypeId, value);
        }

        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        public double Param1
        {
            get => _param1;
            set => SetProperty(ref _param1, value);
        }

        public double Param2
        {
            get => _param2;
            set => SetProperty(ref _param2, value);
        }

        public int Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public ICommand CalculateCommand { get; }

        private async Task LoadReferenceDataAsync()
        {
            try
            {
                var productTypes = await _apiService.GetProductTypesAsync();
                var materialTypes = await _apiService.GetMaterialTypesAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
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
                MessageBox.Show($"Ошибка загрузки справочников: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CalculateAsync()
        {
            try
            {
                if (SelectedProductTypeId == 0 || SelectedMaterialTypeId == 0 || Quantity <= 0 || Param1 <= 0 || Param2 <= 0)
                {
                    MessageBox.Show("Заполните все поля корректными значениями", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Result = await _apiService.CalculateRawMaterialAsync(
                    SelectedProductTypeId, SelectedMaterialTypeId, Quantity, Param1, Param2);

                if (Result == -1)
                {
                    MessageBox.Show("Ошибка расчета. Проверьте введенные данные", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка расчета: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}