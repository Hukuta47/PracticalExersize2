using ComfortDesktop.Models;
using ComfortDesktop.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ComfortDesktop.ViewModels
{
    public class AnalysisViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private ObservableCollection<ProductionSummary> _productionSummary;
        private ObservableCollection<ProductTotalTime> _productsTotalTime;

        public AnalysisViewModel(IApiService apiService)
        {
            _apiService = apiService;
            _productionSummary = new ObservableCollection<ProductionSummary>();
            _productsTotalTime = new ObservableCollection<ProductTotalTime>();

            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
            _ = LoadDataAsync();
        }

        public ObservableCollection<ProductionSummary> ProductionSummary
        {
            get => _productionSummary;
            set => SetProperty(ref _productionSummary, value);
        }

        public ObservableCollection<ProductTotalTime> ProductsTotalTime
        {
            get => _productsTotalTime;
            set => SetProperty(ref _productsTotalTime, value);
        }

        public ICommand LoadDataCommand { get; }

        private async System.Threading.Tasks.Task LoadDataAsync()
        {
            try
            {
                var summary = await _apiService.GetProductionSummaryAsync();
                var totalTime = await _apiService.GetProductsTotalTimeAsync();

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    ProductionSummary.Clear();
                    foreach (var item in summary)
                        ProductionSummary.Add(item);

                    ProductsTotalTime.Clear();
                    foreach (var item in totalTime)
                        ProductsTotalTime.Add(item);
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