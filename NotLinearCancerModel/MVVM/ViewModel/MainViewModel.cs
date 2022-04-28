
using NotLinearCancerModel.Core;

namespace NotLinearCancerModel.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        
        public RelayCommand CalculateOneViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }

        public CalculateOneViewModel CalculateOneVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            CalculateOneVM = new CalculateOneViewModel();

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            CalculateOneViewCommand = new RelayCommand(o =>
            {
                CurrentView = CalculateOneVM;
            });
        }
    }
}
