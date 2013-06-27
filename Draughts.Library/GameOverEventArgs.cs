using System;

namespace Draughts.Library
{

    public class GameOverEventArgs : EventArgs
    {

        private Players playerWin;

        public GameOverEventArgs(Players playerWin)
        {
            this.playerWin = playerWin;
        }

        public Players PlayerWin
        {
            get
            {
                return playerWin;
            }
        }

    }

}
