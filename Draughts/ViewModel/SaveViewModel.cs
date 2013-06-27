using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Draughts.Command;

namespace Draughts.ViewModel
{

    public class SaveViewModel : ViewModelBase
    {

        private Window view;

        private string name;

        private RelayCommand saveCommand;

        public SaveViewModel()
        {
            for (int i = 0; ; i++)
            {
                name = string.Format("{0}-{1}-{2}-{3}", DateTime.Today.Day, DateTime.Today.Month, DateTime.Today.Year, i);
                if (!File.Exists("Saves\\" + name + ".sav"))
                    break;
            }

            if (!Directory.Exists("Saves"))
                Directory.CreateDirectory("Saves");
        }

        private void SaveCommand_Execute(object o)
        {
            view.DialogResult = true;
            view.Close();
        }

        private bool SaveCommand_CanExecute(object o)
        {
            return !string.IsNullOrWhiteSpace(name) && !File.Exists("Saves\\" + name + ".sav");
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

        public ICommand SaveCommand
        {
            get { return saveCommand ?? (saveCommand = new RelayCommand(SaveCommand_Execute, SaveCommand_CanExecute)); }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

    }

}
