using Prism.Commands;
using Prism.Mvvm;
using ProfilesCalculator.Core;
using ProfilesCalculator.Core.Models;
using ProfilesCalculator.Shared.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace ProfilesCalculator.MainModule.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private string _title = "Profile";
        private DelegateCommand _openCommand;
        private DelegateCommand _saveCommand;

        public string Title
        {
            get => _title;
            set => _ = SetProperty(ref _title, value);
        }

        private int _baseProfileLenght = 12000;

        public int BaseProfileLenght
        {
            get => _baseProfileLenght;
            set => _ = SetProperty(ref _baseProfileLenght, value);
        }

        private int _baseProfileCutWaste = 5;

        public int BaseProfileCutWaste
        {
            get => _baseProfileCutWaste;
            set => _ = SetProperty(ref _baseProfileCutWaste, value);
        }


        private IEnumerable<NewProfile> _newProfiles;

        public IEnumerable<NewProfile> NewProfiles
        {
            get => _newProfiles;
            set => _ = SetProperty(ref _newProfiles, value);
        }


        private NewProfile _selectedNewProfile;

        public NewProfile SelectedNewProfile
        {
            get => _selectedNewProfile;
            set => _ = SetProperty(ref _selectedNewProfile, value);
        }

        private string _selectedPath;
        public string SelectedPath
        {
            get => _selectedPath;
            set
            {
                _ = SetProperty(ref _selectedPath, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand OpenCommand => _openCommand ??= new DelegateCommand(ExecuteOpenCommand, CanExecuteOpenCommand);

        public DelegateCommand SaveCommand => _saveCommand ??= new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand);

        void ExecuteOpenCommand()
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();

            SelectedPath = dialog.FileName;
            ExcelFileReader reader = new(SelectedPath);
            try
            {
                reader.InitExcel();
            }
            catch (FileNotFoundException)
            {
                _ = System.Windows.MessageBox.Show("Cannot read Excel file.", "Oops, something went wrong: (", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var profileList = reader.GetData();
            IProfileGenerator generator = new CutProfiles(ref profileList, BaseProfileLenght, BaseProfileCutWaste);
            var newProfiles = generator.GenerateNewProfiles();

            NewProfiles = newProfiles.OrderBy(x => x.WasteLength).ToArray();
        }

        bool CanExecuteOpenCommand() => true;

        private bool CanExecuteSaveCommand() => SelectedPath != null;

        void ExecuteSaveCommand()
        {
            var reader = new ExcelFileReader(SelectedPath);
            reader.StoreInCsvFile(NewProfiles);
            _ = System.Windows.MessageBox.Show("New profiles were saved in this location: " + reader.NewPath);
        }

    }
}
