using System;
using System.Windows;
using Draughts.View;
using Draughts.ViewModel;

namespace Draughts
{

    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            MainViewModel mainViewModel = new MainViewModel();
            MainView mainView = new MainView(mainViewModel);
            mainViewModel.View = mainView;
            mainView.DataContext = mainViewModel;

            mainView.Show();
        }

    }

}
