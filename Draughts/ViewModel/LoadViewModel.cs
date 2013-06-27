using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Draughts.Command;

namespace Draughts.ViewModel
{

    public class LoadViewModel : ViewModelBase
    {

        private Window view;

        private ObservableCollection<string> saves;
        private string selectedSave;

        private RelayCommand loadCommand;
        private RelayCommand deleteCommand;

        public LoadViewModel()
        {
            if (!Directory.Exists("Saves"))
                Directory.CreateDirectory("Saves");

            var files = from file in Directory.GetFiles("Saves", "*.sav", SearchOption.TopDirectoryOnly)
                        select Path.GetFileNameWithoutExtension(file);
            saves = new ObservableCollection<string>(files);

            if (saves.Count > 0)
                selectedSave = saves[0];
        }

        private void LoadCommand_Execute(object o)
        {
            view.DialogResult = true;
            view.Close();
        }

        private bool LoadCommand_CanExecute(object o)
        {
            return selectedSave != null;
        }

        private void DeleteCommand_Execute(object o)
        {
            File.Delete("Saves\\" + selectedSave + ".sav");

            int index = saves.IndexOf(selectedSave);
            saves.Remove(selectedSave);
            if (!string.IsNullOrWhiteSpace(saves[index]))
                SelectedSave = saves[index];
        }

        private bool DeleteCommand_CanExecute(object o)
        {
            return selectedSave != null && saves.Count > 0;
        }

        public ICommand LoadCommand
        {
            get { return loadCommand ?? (loadCommand = new RelayCommand(LoadCommand_Execute, LoadCommand_CanExecute)); }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                       (deleteCommand = new RelayCommand(DeleteCommand_Execute, DeleteCommand_CanExecute));
            }
        }

        public Window View
        {
            get { return view; }
            set
            {
                view = value;
                OnPropertyChanged("View");
            }
        }

        public ObservableCollection<string> Saves
        {
            get { return saves; }
            set
            {
                saves = value;
                OnPropertyChanged("Saves");
            }
        }

        public string SelectedSave
        {
            get { return selectedSave; }
            set
            {
                selectedSave = value;
                OnPropertyChanged("SelectedSave");
            }
        }

    }

}
