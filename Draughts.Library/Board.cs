using System;
using Draughts.Library.Exceptions;

namespace Draughts.Library
{

    public class Board
    {

        private Draught[,] draughts;
        private int leftDraughtsPlayerOne;
        private int leftDraughtsPlayerTwo;

        public Board()
        {
            CreateBoard();
        }

        public Board(Draught[,] draughts, int leftDraughtsPlayerOne, int leftDraughtsPlayerTwo)
        {
            this.draughts = draughts;
            this.leftDraughtsPlayerOne = leftDraughtsPlayerOne;
            this.leftDraughtsPlayerTwo = leftDraughtsPlayerTwo;
        }

        public Draught this[BoardPoint point]
        {
            get { return this[point.Y, point.X]; }
            set { this[point.Y, point.X] = value; }
        }

        public Draught this[int y, int x]
        {
            get
            {
                return draughts[y, x];
            }
            set
            {
                draughts[y, x] = value;
            }
        }

        private void CreatePlayerOneDraught()
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if ((y + x) % 2 != 0)
                    {
                        draughts[y, x] = new Draught(x, y, Players.PlayerOne);
                    }
                }
            }

            leftDraughtsPlayerOne = 12;
        }

        private void CreatePlayerTwoDraught()
        {
            for (int y = 5; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if ((y + x) % 2 != 0)
                    {
                        draughts[y, x] = new Draught(x, y, Players.PlayerTwo);
                    }
                }
            }

            leftDraughtsPlayerTwo = 12;
        }

        public void CreateBoard()
        {
            draughts = new Draught[8, 8];
            CreatePlayerOneDraught();
            CreatePlayerTwoDraught();
        }

        public void Move(Draught draught, BoardPoint point)
        {
            if (draught == null)
                throw new ArgumentNullException();

            Move(draught.X, draught.Y, point.X, point.Y);
        }

        public void Move(Draught draught, int x, int y)
        {
            if (draught == null)
                throw new ArgumentNullException();

            Move(draught.X, draught.Y, x, y);
        }

        public void Move(int oldX, int oldY, int newX, int newY)
        {
            if (((oldX + oldY) % 2 == 0) || ((newX + newY) % 2 == 0))
                throw new BoardException("Даны не верные координаты.");
            if (draughts[oldY, oldX] == null)
                throw new BoardException("Начальная клетка пуста.");
            if (draughts[newY, newX] != null)
                throw new BoardException("Конечная клетка занята.");

            draughts[newY, newX] = draughts[oldY, oldX];
            draughts[newY, newX].X = newX;
            draughts[newY, newX].Y = newY;
            draughts[oldY, oldX] = null;
        }

        public void Remove(Draught draught)
        {
            Remove(draught.X, draught.Y);
        }

        public void Remove(BoardPoint point)
        {
            Remove(point.X, point.Y);
        }

        public void Remove(int x, int y)
        {
            if ((x + y) % 2 == 0)
                throw new BoardException("Даны не верные координаты.");
            if (draughts[y, x] == null)
                throw new BoardException("Клетка пуста.");

            if (draughts[y, x].Player == Players.PlayerOne)
                leftDraughtsPlayerOne--;
            else
                leftDraughtsPlayerTwo--;

            draughts[y, x] = null;
        }

        public Draught[,] Draughts
        {
            get
            {
                return draughts;
            }
        }

        public int LeftDraughtPlayerOne
        {
            get
            {
                return leftDraughtsPlayerOne;
            }
        }

        public int LeftDraughtPlayerTwo
        {
            get
            {
                return leftDraughtsPlayerTwo;
            }
        }

    }

}
