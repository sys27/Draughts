using Draughts.Library;
using Draughts.Library.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Draughts.Test
{

    /// <summary>
    ///Это класс теста для BoardTest, в котором должны
    ///находиться все модульные тесты BoardTest
    ///</summary>
    [TestClass]
    public class BoardTest
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
        ///Тест для Move
        ///</summary>
        [TestMethod]
        public void MoveTest()
        {
            Board target = new Board();
            int oldX = 3;
            int oldY = 2;
            Draught draught = target[oldY, oldX];

            int newX = 4;
            int newY = 3;
            target.Move(oldX, oldY, newX, newY);
            Assert.AreEqual<Draught>(draught, target[newY, newX]);
        }

        /// <summary>
        ///Тест для Move
        ///</summary>
        [TestMethod]
        public void MoveTest1()
        {
            Board target = new Board();
            int oldX = 3;
            int oldY = 2;
            Draught draught = target[oldY, oldX];

            int newX = 4;
            int newY = 3;
            target.Move(draught, newX, newY);
            Assert.AreEqual<Draught>(draught, target[newY, newX]);
        }

        /// <summary>
        ///Тест для Move, первые два параметра (oldX, oldY) не потодают в черный квадрат.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BoardException))]
        public void MoveTest2()
        {
            Board target = new Board();
            int oldX = 3;
            int oldY = 3;
            Draught draught = target[oldY, oldX];

            int newX = 4;
            int newY = 3;
            target.Move(oldX, oldY, newX, newY);
        }

        /// <summary>
        ///Тест для Move, вторые два параметра (newX, newY) не потодают в черный квадрат.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BoardException))]
        public void MoveTest3()
        {
            Board target = new Board();
            int oldX = 3;
            int oldY = 2;
            Draught draught = target[oldY, oldX];

            int newX = 4;
            int newY = 4;
            target.Move(oldX, oldY, newX, newY);
        }

        /// <summary>
        ///Тест для Move, начальное положение пусто.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BoardException))]
        public void MoveTest4()
        {
            Board target = new Board();
            int oldX = 3;
            int oldY = 2;
            target[oldY, oldX] = null;

            int newX = 4;
            int newY = 3;
            target.Move(oldX, oldY, newX, newY);
        }

        /// <summary>
        ///Тест для Move, конечное положение не пусто.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BoardException))]
        public void MoveTest5()
        {
            Board target = new Board();
            int oldX = 2;
            int oldY = 1;

            int newX = 3;
            int newY = 2;
            target.Move(oldX, oldY, newX, newY);
        }

        /// <summary>
        ///Тест для Move, одной из перегрузок метода передаётся значение null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MoveTest6()
        {
            Board target = new Board();
            int newX = 3;
            int newY = 2;
            target.Move(null, newX, newY);
        }

        /// <summary>
        ///Тест для Move
        ///</summary>
        [TestMethod]
        public void MoveTest7()
        {
            Board target = new Board();
            BoardPoint oldPoint = new BoardPoint(2, 3);
            Draught draught = target[oldPoint];

            BoardPoint newPoint = new BoardPoint(3, 4);
            target.Move(draught, newPoint);
            Assert.AreEqual<Draught>(draught, target[newPoint]);
        }

        /// <summary>
        /// Пестирует перегрузку метода Move(Draught, BoardPoint).
        /// </summary>
        [TestMethod]
        public void MoveTest8()
        {
            Board board = new Board();
            Draught dr = board[2, 1];
            BoardPoint point = new BoardPoint(3, 2);
            board.Move(dr, point);

            Assert.AreEqual(board[point], dr);
        }

        /// <summary>
        /// Пестирует перегрузку метода Move(Draught, BoardPoint).
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MoveTest9()
        {
            Board board = new Board();
            Draught dr = null;
            board.Move(dr, new BoardPoint(3, 2));
        }

        /// <summary>
        ///Тест для Remove
        ///</summary>
        [TestMethod]
        public void RemoveTest()
        {
            Board target = new Board();
            int x = 3;
            int y = 2;
            target.Remove(x, y);
            Assert.IsNull(target[y, x]);
        }

        /// <summary>
        ///Тест для Remove
        ///</summary>
        [TestMethod]
        public void RemoveTest1()
        {
            Board target = new Board();
            int x = 3;
            int y = 2;
            Draught draught = target[y, x];
            target.Remove(draught);
            Assert.IsNull(target[y, x]);
        }

        /// <summary>
        ///Тест для Remove, шашка находится на черной клетке.
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(BoardException))]
        public void RemoveTest2()
        {
            Board target = new Board();
            int x = 3;
            int y = 3;
            target.Remove(x, y);
        }

        /// <summary>
        ///Тест для Remove, указанная клетка пуста.
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(BoardException))]
        public void RemoveTest3()
        {
            Board target = new Board();
            int x = 3;
            int y = 2;
            target[y, x] = null;
            target.Remove(x, y);
        }

        /// <summary>
        ///Тест для Remove, уменьшение счетчика шашек.
        ///</summary>
        [TestMethod]
        public void RemoveTest4()
        {
            Board target = new Board();
            int x = 3;
            int y = 2;
            target.Remove(x, y);
            Assert.AreEqual<int>(11, target.LeftDraughtPlayerTwo);
        }

        /// <summary>
        ///Тест для Remove
        ///</summary>
        [TestMethod]
        public void RemoveTest5()
        {
            Board target = new Board();
            BoardPoint point = new BoardPoint(2, 3);
            target.Remove(point);

            Assert.IsNull(target[point]);
        }

        /// <summary>
        ///Тест для Item
        ///</summary>
        [TestMethod]
        public void ItemTest()
        {
            Board target = new Board();
            int y = 5;
            int x = 4;
            Draught expected = target[y, x];
            Assert.AreEqual<Draught>(expected, target[y, x]);

            target[y, x] = expected;
            Assert.AreEqual<Draught>(target[y, x], expected);
        }

        /// <summary>
        ///Тест для Item, выход индекса за пределы при получении значения.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void ItemTest1()
        {
            Board target = new Board();
            int y = -1;
            int x = 9;
            Draught expected = target[y, x];
            target[y, x] = expected;
        }

        /// <summary>
        ///Тест для Item, выход индекса за пределы при установке значения.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void ItemTest2()
        {
            Board target = new Board();
            int y = -1;
            int x = 9;
            target[y, x] = new Draught(9, -1, DraughtType.None, Players.PlayerOne);
        }

        /// <summary>
        ///Тест для Item
        ///</summary>
        [TestMethod]
        public void ItemTest3()
        {
            Board target = new Board();
            BoardPoint point = new BoardPoint(5, 4);
            Draught expected = target[point];
            Assert.AreEqual<Draught>(expected, target[point]);

            target[point] = expected;
            Assert.AreEqual<Draught>(target[point], expected);
        }

    }

}
