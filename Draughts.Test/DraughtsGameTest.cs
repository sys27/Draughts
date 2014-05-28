using Draughts.Library;
using Draughts.Library.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Draughts.Test
{

    /// <summary>
    ///Это класс теста для DraughtsGameTest, в котором должны
    ///находиться все модульные тесты DraughtsGameTest
    ///</summary>
    [TestClass]
    public class DraughtsGameTest
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Получает или устанавливает контекст теста, в котором предоставляются
        ///сведения о текущем тестовом запуске и обеспечивается его функциональность.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Дополнительные атрибуты теста
        // 
        //При написании тестов можно использовать следующие дополнительные атрибуты:
        //
        //ClassInitialize используется для выполнения кода до запуска первого теста в классе
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ClassCleanup используется для выполнения кода после завершения работы всех тестов в классе
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //TestInitialize используется для выполнения кода перед запуском каждого теста
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //TestCleanup используется для выполнения кода после завершения каждого теста
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///Тест для Turn, обычный ход.
        ///</summary>
        [TestMethod]
        public void TurnTest()
        {
            DraughtsGame target = new DraughtsGame();
            int startX = 2;
            int startY = 5;
            Draught draught = target.GameBoard[startY, startX];

            int endX = 3;
            int endY = 4;
            target.Turn(startX, startY, endX, endY);

            Assert.AreEqual(draught, target.GameBoard[endY, endX]);
        }

        /// <summary>
        ///Тест для Turn, обычный ход.
        ///</summary>
        [TestMethod]
        public void TurnTest1()
        {
            DraughtsGame target = new DraughtsGame();
            BoardPoint startPoint = new BoardPoint(5, 2);
            Draught draught = target.GameBoard[startPoint];

            BoardPoint endPoint = new BoardPoint(4, 3);
            target.Turn(startPoint, endPoint);

            Assert.AreEqual(draught, target.GameBoard[endPoint]);
        }

        /// <summary>
        /// Ход шашкой назад.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DraughtsGameException))]
        public void TurnBackTest()
        {
            DraughtsGame game = new DraughtsGame();

            game.Turn(3, 2, 4, 3);
            game.Turn(2, 5, 1, 4);
            game.Turn(4, 3, 3, 2);
        }

        /// <summary>
        /// Тест взятия шашки.
        /// </summary>
        [TestMethod]
        public void KillTest()
        {
            DraughtsGame game = new DraughtsGame();
            Draught dr = game.GameBoard[5, 4];

            game.Turn(4, 5, 5, 4);
            game.Turn(3, 2, 4, 3);
            game.Turn(5, 4, 3, 2);

            Assert.AreEqual<Draught>(dr, game.GameBoard[2, 3]);
            Assert.AreEqual(11, game.GameBoard.LeftDraughtPlayerTwo);
        }

        /// <summary>
        /// Ход дамкой.
        /// </summary>
        [TestMethod]
        public void QueenTurnTest()
        {
            Draught[,] draughts = new Draught[8, 8];
            Draught dr = new Draught(1, 0, DraughtType.Queen, Players.PlayerOne);
            draughts[0, 1] = dr;
            Board board = new Board(draughts, 1, 1);
            DraughtsGame game = new DraughtsGame { GameBoard = board };

            // вниз-вправо
            game.Turn(1, 0, 4, 3);
            Assert.AreEqual<Draught>(dr, game.GameBoard[3, 4]);
            game.CurrentTurn = Players.PlayerOne;

            // вверх-вправо
            game.Turn(4, 3, 6, 1);
            Assert.AreEqual<Draught>(dr, game.GameBoard[1, 6]);
            game.CurrentTurn = Players.PlayerOne;

            // вниз-влево
            game.Turn(6, 1, 2, 5);
            Assert.AreEqual<Draught>(dr, game.GameBoard[5, 2]);
            game.CurrentTurn = Players.PlayerOne;

            // вверх-влево
            game.Turn(2, 5, 0, 3);
            Assert.AreEqual<Draught>(dr, game.GameBoard[3, 0]);
            game.CurrentTurn = Players.PlayerOne;
        }

        /// <summary>
        /// Ход дамкой, назад
        /// </summary>
        [TestMethod]
        public void QueenTurnBackTest()
        {
            Draught[,] draughts = new Draught[8, 8];
            Draught dr = new Draught(4, 3, DraughtType.Queen, Players.PlayerOne); ;
            draughts[3, 4] = dr;
            Board board = new Board(draughts, 1, 0);
            DraughtsGame game = new DraughtsGame { GameBoard = board };

            game.Turn(4, 3, 1, 0);
            Assert.AreEqual<Draught>(dr, game.GameBoard[0, 1]);
        }

        /// <summary>
        /// Ход дамкой не по диагонали.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DraughtsGameException))]
        public void QueenWrongTurnTest()
        {
            Draught[,] draughts = new Draught[8, 8];
            Draught dr = new Draught(1, 0, DraughtType.Queen, Players.PlayerOne); ;
            draughts[0, 1] = dr;
            Board board = new Board(draughts, 1, 0);
            DraughtsGame game = new DraughtsGame { GameBoard = board };

            game.Turn(1, 0, 6, 7);
        }

        /// <summary>
        /// Взяти дамкой шашки.
        /// </summary>
        [TestMethod]
        public void QueenKillTest()
        {
            Draught[,] draughts = new Draught[8, 8];
            Draught quuen = new Draught(1, 0, DraughtType.Queen, Players.PlayerOne); ;
            draughts[0, 1] = quuen;
            Draught dr = new Draught(3, 2, DraughtType.None, Players.PlayerTwo);
            draughts[2, 3] = dr;

            Board board = new Board(draughts, 1, 0);
            DraughtsGame game = new DraughtsGame { GameBoard = board };

            game.Turn(1, 0, 7, 6);
            Assert.AreEqual<Draught>(quuen, game.GameBoard[6, 7]);
        }

        /// <summary>
        /// Взяти дамкой 2-х вражеских шашек.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DraughtsGameException))]
        public void QueenKill2DraughtsTest()
        {
            Draught[,] draughts = new Draught[8, 8];
            Draught quuen = new Draught(1, 0, DraughtType.Queen, Players.PlayerOne); ;
            draughts[0, 1] = quuen;
            Draught dr = new Draught(3, 2, DraughtType.None, Players.PlayerTwo);
            draughts[2, 3] = dr;
            Draught dr1 = new Draught(4, 3, DraughtType.None, Players.PlayerTwo);
            draughts[3, 4] = dr1;

            Board board = new Board(draughts, 1, 0);
            DraughtsGame game = new DraughtsGame { GameBoard = board };

            game.Turn(1, 0, 7, 6);
            Assert.AreEqual(quuen, game.GameBoard[6, 7]);
        }

        /// <summary>
        ///Тест для Turn, ход не того игрока.
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(DraughtsGameException))]
        public void WrongPlayerTurnTest()
        {
            DraughtsGame target = new DraughtsGame();

            target.Turn(3, 2, 4, 3);
            target.Turn(4, 3, 5, 4);
        }

        /// <summary>
        ///Тест для Turn, перемешение на туже клетку.
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(DraughtsGameException))]
        public void StartEqualEndTurnTest()
        {
            DraughtsGame target = new DraughtsGame();
            int startX = 4;
            int startY = 5;
            Draught draught = target.GameBoard[startY, startX];

            int endX = 4;
            int endY = 5;
            target.Turn(startX, startY, endX, endY);
        }

        /// <summary>
        ///Тест для Turn, начальная клетка пуста.
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(DraughtsGameException))]
        public void StartCellIsNullTurnTest()
        {
            DraughtsGame target = new DraughtsGame();
            int startX = 4;
            int startY = 5;
            target.GameBoard[startY, startX] = null;

            int endX = 5;
            int endY = 4;
            target.Turn(startX, startY, endX, endY);
        }

        /// <summary>
        ///Тест для Turn, конечная клетка занята.
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(DraughtsGameException))]
        public void EndCellIsNotNullTurnTest()
        {
            Draught[,] draughts = new Draught[8, 8];
            draughts[0, 1] = new Draught(1, 0, DraughtType.None, Players.PlayerOne);
            draughts[1, 2] = new Draught(2, 1, DraughtType.None, Players.PlayerOne);
            draughts[7, 1] = new Draught(0, 7, DraughtType.None, Players.PlayerTwo);
            Board board = new Board(draughts, 2, 1);
            DraughtsGame game = new DraughtsGame() { GameBoard = board, CurrentTurn = Players.PlayerOne };

            game.Turn(1, 0, 2, 1);
        }

        /// <summary>
        /// Проверка, становится ли дамкой пешка на своём поле.
        /// </summary>
        [TestMethod]
        public void DraughtToQueen()
        {
            Draught[,] draughts = new Draught[8, 8];
            draughts[1, 2] = new Draught(2, 1, DraughtType.None, Players.PlayerTwo);
            Draught nonQueen = new Draught(3, 2, DraughtType.None, Players.PlayerOne);
            draughts[2, 3] = nonQueen;

            Board board = new Board(draughts, 1, 1);
            DraughtsGame game = new DraughtsGame() { GameBoard = board };
            game.Turn(3, 2, 1, 0);

            Assert.AreEqual(DraughtType.None, nonQueen.Type);
        }

        [TestMethod]
        [ExpectedException(typeof(DraughtsGameException))]
        public void KillOwnDraught()
        {
            Draught[,] draughts = new Draught[8, 8];
            draughts[0, 1] = new Draught(1, 0, DraughtType.Queen, Players.PlayerOne);
            draughts[1, 2] = new Draught(2, 1, DraughtType.None, Players.PlayerOne);
            draughts[7, 0] = new Draught(0, 7, DraughtType.None, Players.PlayerOne);
            Board board = new Board(draughts, 2, 1);
            DraughtsGame game = new DraughtsGame() { GameBoard = board, CurrentTurn = Players.PlayerOne };
            game.Turn(1, 0, 3, 2);
        }

    }

}
