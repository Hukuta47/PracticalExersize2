using ComfortDesktop.Models;
using ComfortDesktop.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ComfortDesktop.ViewModels
{
    public class ManufacturingViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private ObservableCollection<ManufacturingData> _manufacturingData;

        public ManufacturingViewModel(IApiService apiService)
        {
            _apiService = apiService;
            _manufacturingData = new ObservableCollection<ManufacturingData>();

            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
            _ = LoadDataAsync();
        }

        public ObservableCollection<ManufacturingData> ManufacturingData
        {
            get => _manufacturingData;
            set => SetProperty(ref _manufacturingData, value);
        }

        public ICommand LoadDataCommand { get; }

        private async System.Threading.Tasks.Task LoadDataAsync()
        {
            try
            {
                var data = await _apiService.GetManufacturingDataAsync();
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    ManufacturingData.Clear();
                    foreach (var item in data)
                        ManufacturingData.Add(item);
                });
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}