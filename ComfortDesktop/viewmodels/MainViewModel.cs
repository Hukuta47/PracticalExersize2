using ComfortDesktop.Services;
using ComfortDesktop.ViewModels;
using System.Windows.Input;

namespace ComfortDesktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private BaseViewModel _currentView;

        public MainViewModel()
        {
            _apiService = new ApiService();

            // Инициализируем стартовую ViewModel
            _currentView = new ProductsViewModel(_apiService);

            ShowProductsCommand = new RelayCommand(() => CurrentView = new ProductsViewModel(_apiService));
            ShowManufacturingCommand = new RelayCommand(() => CurrentView = new ManufacturingViewModel(_apiService));
            ShowAnalysisCommand = new RelayCommand(() => CurrentView = new AnalysisViewModel(_apiService));
            ShowCalculationCommand = new RelayCommand(() => CurrentView = new CalculationViewModel(_apiService));
        }

        public BaseViewModel CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand ShowProductsCommand { get; }
        public ICommand ShowManufacturingCommand { get; }
        public ICommand ShowAnalysisCommand { get; }
        public ICommand ShowCalculationCommand { get; }
    }

    public abstract class BaseViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly System.Action _execute;
        private readonly System.Func<bool> _canExecute;

        public RelayCommand(System.Action execute, System.Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new System.ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event System.EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => _execute();
    }
}