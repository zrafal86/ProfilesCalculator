using Prism.Mvvm;

namespace ProfilesCalculator.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Profiles Calculator";
        public string Title
        {
            get => _title;
            set => _ = SetProperty(ref _title, value);
        }

        public MainWindowViewModel()
        {

        }
    }
}
