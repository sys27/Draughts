using System;
using Draughts.Library.Exceptions;

namespace Draughts.Library
{

    public class DraughtsGame
    {

        private Board board;
        private Players currentPlayer;

        private Draught canDraugtKill;
        private bool canKill;

        public event EventHandler BoardChanged;
        public event EventHandler<GameOverEventArgs> GameOver;

        public DraughtsGame()
        {
            board = new Board();
            canKill = false;
            StartNew();
        }

        protected virtual void OnBoardChanged()
        {
            if (BoardChanged != null)
                BoardChanged(this, EventArgs.Empty);
        }

        protected virtual void OnGameOver(Players playerWin)
        {
            if (GameOver != null)
                GameOver(this, new GameOverEventArgs(playerWin));
        }

        public static void Save(DraughtsGame game, string path)
        {
            BoardSerializer.Serialize(game, path);
        }

        public static DraughtsGame Load(string path)
        {
            return BoardSerializer.Deserialize(path);
        }

        private void ChangeCurrentPlayer()
        {
            if (currentPlayer == Players.PlayerOne)
                currentPlayer = Players.PlayerTwo;
            else
                currentPlayer = Players.PlayerOne;
        }

        private void TurnMove(Draught draught, int endX, int endY)
        {
            if (canKill)
            {
                board.Remove(canDraugtKill);
                canKill = false;
            }

            DraughtIsQueen(draught, endY);
            board.Move(draught, endX, endY);

            ChangeCurrentPlayer();
            OnBoardChanged();
            CheckWin();
        }

        private void TurnKill(Draught draught, int endX, int endY, int centerX, int centerY)
        {
            DraughtIsQueen(draught, endY);
            board.Move(draught, endX, endY);
            board.Remove(centerX, centerY);
            canKill = false;

            if (!DraughtCanKill(draught, endX, endY))
                ChangeCurrentPlayer();
            OnBoardChanged();
            CheckWin();
        }

        private void DraughtIsQueen(Draught draught, int endY)
        {
            if (draught.Type != DraughtType.Queen &&
                (draught.Player == Players.PlayerOne && endY == 7) ||
                (draught.Player == Players.PlayerTwo && endY == 0))
            {
                draught.Type = DraughtType.Queen;
            }
        }

        private bool DraughtCanKill(Draught draught, int startX, int startY)
        {
            if ((startY - 1 >= 0 && startX + 1 <= 7) &&
                (startY - 2 >= 0 && startX + 2 <= 7) &&
                board[startY - 1, startX + 1] != null &&
                board[startY - 1, startX + 1].Player != draught.Player &&
                board[startY - 2, startX + 2] == null)
            {
                canKill = true;
                canDraugtKill = draught;
            }
            else if ((startY - 1 >= 0 && startX - 1 >= 0) &&
                (startY - 2 >= 0 && startX - 2 >= 0) &&
                board[startY - 1, startX - 1] != null &&
                board[startY - 1, startX - 1].Player != draught.Player &&
                board[startY - 2, startX - 2] == null)
            {
                canKill = true;
                canDraugtKill = draught;
            }
            else if ((startY + 1 <= 7 && startX - 1 >= 0) &&
                (startY + 2 <= 7 && startX - 2 >= 0) &&
                board[startY + 1, startX - 1] != null &&
                board[startY + 1, startX - 1].Player != draught.Player &&
                board[startY + 2, startX - 2] == null)
            {
                canKill = true;
                canDraugtKill = draught;
            }
            else if ((startY + 1 <= 7 && startX + 1 <= 7) &&
                (startY + 2 <= 7 && startX + 2 <= 7) &&
                board[startY + 1, startX + 1] != null &&
                board[startY + 1, startX + 1].Player != draught.Player &&
                board[startY + 2, startX + 2] == null)
            {
                canKill = true;
                canDraugtKill = draught;
            }

            return canKill;
        }

        private void AllDraughtsCanKill()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if ((y + x) % 2 != 0 && board[y, x] != null && board[y, x].Player == currentPlayer)
                    {
                        if (DraughtCanKill(board[y, x], x, y))
                            return;
                    }
                }
            }
        }

        private void CheckWin()
        {
            if (board.LeftDraughtPlayerOne == 0)
            {
                OnGameOver(Players.PlayerOne);
            }
            else if (board.LeftDraughtPlayerTwo == 0)
            {
                OnGameOver(Players.PlayerTwo);
            }
        }

        private BoardPoint CheckRange(Draught queen, int fromX, int toX, int fromY, int toY)
        {
            int draughtsCount = 0;
            BoardPoint point = new BoardPoint(-1, -1);

            for (int y = fromY; y <= toY; y++)
            {
                for (int x = fromX; x <= toX; x++)
                {
                    if (board[y, x] != null)
                    {
                        if (board[y, x].Player == queen.Player)
                            throw new DraughtsGameException("Вы не можете побить свою шашку.");

                        draughtsCount++;
                        if (draughtsCount > 1)
                        {
                            throw new DraughtsGameException("Бить больше, чем одну шашку нельзя.");
                        }
                        point = new BoardPoint(y, x);
                    }
                }
            }

            return point;
        }

        public void StartNew()
        {
            board.CreateBoard();
            currentPlayer = Players.PlayerOne;

            OnBoardChanged();
        }

        public void Turn(BoardPoint startPoint, BoardPoint endPoint)
        {
            Turn(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
        }

        public void Turn(int startX, int startY, int endX, int endY)
        {
            if (startX == endX || startY == endY)
                throw new DraughtsGameException("Перемещение на туже клетку не возможно.");
            if (board[startY, startX] == null)
                throw new DraughtsGameException("Начальная клетка пуста.");
            if (board[endY, endX] != null)
                throw new DraughtsGameException("Конечная клетка занята.");

            Draught draught = board[startY, startX];

            if (draught.Player != currentPlayer)
                throw new DraughtsGameException(string.Format("Игрок {0} уже ходил.", draught.Player == Players.PlayerOne ? 1 : 2));

            if (draught.Type == DraughtType.None)
            {
                TurnNormalDraught(draught, endX, endY);
            }
            else if (draught.Type == DraughtType.Queen)
            {
                TurnQueenDraught(draught, endX, endY);
            }
        }

        private void TurnQueenDraught(Draught draught, int endX, int endY)
        {
            if (Math.Abs(endX - draught.X) == Math.Abs(endY - draught.Y))
            {
                int count = Math.Abs(endX - draught.X) - 1;
                BoardPoint point = new BoardPoint();

                if (draught.X < endX && draught.Y > endY) // вверх-вправо
                {
                    point = CheckRange(draught, draught.X + 1, draught.X + count, endY + 1, endY + count);
                }
                else if (draught.X > endX && draught.Y > endY) // вверх-влево
                {
                    point = CheckRange(draught, endX + 1, endX + count, endY + 1, endY + count);
                }
                else if (draught.X > endX && draught.Y < endY) // вниз-влево
                {
                    point = CheckRange(draught, endX + 1, endX + count, draught.Y + 1, draught.Y + count);
                }
                else if (draught.X < endX && draught.Y < endY) // вниз-вправо
                {
                    point = CheckRange(draught, draught.X + 1, draught.X + count, draught.Y + 1, draught.Y + count);
                }

                if (point.X != -1 && point.Y != -1)
                {
                    TurnKill(draught, endX, endY, point.X, point.Y);
                }
                else
                {
                    TurnMove(draught, endX, endY);
                }
            }
            else
            {
                throw new DraughtsGameException("Неверный ход дамкой.");
            }
        }

        private void TurnNormalDraught(Draught draught, int endX, int endY)
        {
            AllDraughtsCanKill();
            if ((draught.Player == Players.PlayerOne && ((draught.X - 1 == endX && draught.Y + 1 == endY) || (draught.X + 1 == endX && draught.Y + 1 == endY))) ||
            (draught.Player == Players.PlayerTwo && ((draught.X - 1 == endX && draught.Y - 1 == endY) || (draught.X + 1 == endX && draught.Y - 1 == endY))))
            {
                TurnMove(draught, endX, endY);
            }
            else
            {
                int centerX = 0;
                int centerY = 0;

                if (draught.X + 2 == endX && draught.Y - 2 == endY) // вверх-вправо
                {
                    centerX = draught.X + 1;
                    centerY = draught.Y - 1;
                }
                else if (draught.X - 2 == endX && draught.Y - 2 == endY) // вверх-влево
                {
                    centerX = draught.X - 1;
                    centerY = draught.Y - 1;
                }
                else if (draught.X - 2 == endX && draught.Y + 2 == endY) // вниз-влево
                {
                    centerX = draught.X - 1;
                    centerY = draught.Y + 1;
                }
                else if (draught.X + 2 == endX && draught.Y + 2 == endY) // вниз-вправо
                {
                    centerX = draught.X + 1;
                    centerY = draught.Y + 1;
                }
                else
                {
                    throw new DraughtsGameException("Неверный ход.");
                }

                if (board[centerY, centerX] != null && board[centerY, centerX].Player != draught.Player)
                {
                    TurnKill(draught, endX, endY, centerX, centerY);
                }
            }
        }

        public Board GameBoard
        {
            get { return board; }
            set { board = value; }
        }

        public Players CurrentTurn
        {
            get
            {
                return currentPlayer;
            }
            set
            {
                currentPlayer = value;
            }
        }

    }

}
