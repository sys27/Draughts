using System;
using System.Windows;
using System.Windows.Input;
using Draughts.Command;
using Draughts.Library;
using Draughts.Library.Exceptions;
using Draughts.Resources;
using Draughts.View;

namespace Draughts.ViewModel
{

    public class MainViewModel : ViewModelBase
    {

        private Window view;
        private DraughtsGame game;

        private string currentTurn;
        private string leftDraughts;

        private RelayCommand newGameCommand;
        private RelayCommand saveGameCommand;
        private RelayCommand loadGameCommand;
        private RelayCommand exitCommand;

        public MainViewModel()
        {
            game = new DraughtsGame();
            game.BoardChanged += Game_BoardChanged;
            game.GameOver += GameOver;

            currentTurn = string.Format(Resource.CurrentTurnText, game.CurrentTurn == Players.PlayerOne ? Resource.PlayerOneText : Resource.PlayerTwoText);
            leftDraughts = string.Format(Resource.LeftText, game.GameBoard.LeftDraughtPlayerOne, game.GameBoard.LeftDraughtPlayerTwo);
        }

        private void Game_BoardChanged(object o, EventArgs args)
        {
            CurrentTurn = string.Format(Resource.CurrentTurnText, game.CurrentTurn == Players.PlayerOne ? Resource.PlayerOneText : Resource.PlayerTwoText);
            LeftDraughts = string.Format(Resource.LeftText, game.GameBoard.LeftDraughtPlayerOne, game.GameBoard.LeftDraughtPlayerTwo);

            OnPropertyChanged("BoardChanged");
        }

        private void GameOver(object o, GameOverEventArgs args)
        {
            string text = string.Format(Resource.GameOverText, game.CurrentTurn == Players.PlayerOne ? Resource.PlayerOneText : Resource.PlayerTwoText);
            currentTurn = text;
            MessageBox.Show(text, Resource.GameOverMessageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void NewGameCommand_Execute(object o)
        {
            game.StartNew();
        }

        private void SaveGameCommand_Execute(object o)
        {
            SaveView saveView = new SaveView { Owner = view };
            SaveViewModel saveViewModel = new SaveViewModel { View = saveView };
            saveView.DataContext = saveViewModel;

            if (saveView.ShowDialog() == true)
            {
                DraughtsGame.Save(game, "Saves\\" + saveViewModel.Name + ".sav");
            }
        }

        private void LoadGameCommand_Execute(object o)
        {
            LoadView loadView = new LoadView { Owner = view };
            LoadViewModel loadViewModel = new LoadViewModel { View = loadView };
            loadView.DataContext = loadViewModel;

            if (loadView.ShowDialog() == true)
            {
                game = DraughtsGame.Load("Saves\\" + loadViewModel.SelectedSave + ".sav");
                game.BoardChanged += Game_BoardChanged;
                game.GameOver += GameOver;
                OnPropertyChanged("Load");

                CurrentTurn = string.Format(Resource.CurrentTurnText, game.CurrentTurn == Players.PlayerOne ? Resource.PlayerOneText : Resource.PlayerTwoText);
                LeftDraughts = string.Format(Resource.LeftText, game.GameBoard.LeftDraughtPlayerOne, game.GameBoard.LeftDraughtPlayerTwo);
            }
        }

        public void Turn(int oldX, int oldY, int currentX, int currentY)
        {
            try
            {
                game.Turn(oldX, oldY, currentX, currentY);
            }
            catch (DraughtsGameException) { }
            catch (BoardException) { }
            finally
            {
                OnPropertyChanged("BoardChanged");
            }
        }

        private void ExitCommand_Execute(object o)
        {
            Application.Current.Shutdown();
        }

        public ICommand NewGameCommand
        {
            get { return newGameCommand ?? (newGameCommand = new RelayCommand(NewGameCommand_Execute)); }
        }

        public ICommand SaveGameCommand
        {
            get { return saveGameCommand ?? (saveGameCommand = new RelayCommand(SaveGameCommand_Execute)); }
        }

        public ICommand LoadGameCommand
        {
            get { return loadGameCommand ?? (loadGameCommand = new RelayCommand(LoadGameCommand_Execute)); }
        }

        public ICommand ExitCommand
        {
            get { return exitCommand ?? (exitCommand = new RelayCommand(ExitCommand_Execute)); }
        }

        public Window View
        {
            get { return view; }
            set { view = value; }
        }

        public DraughtsGame Game
        {
            get
            {
                return game;
            }
        }

        public string CurrentTurn
        {
            get { return currentTurn; }
            set
            {
                currentTurn = value;
                OnPropertyChanged("CurrentTurn");
            }
        }

        public string LeftDraughts
        {
            get { return leftDraughts; }
            set
            {
                leftDraughts = value;
                OnPropertyChanged("LeftDraughts");
            }
        }

    }

}
