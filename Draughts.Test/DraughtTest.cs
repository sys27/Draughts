using Draughts.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Draughts.Test
{

    /// <summary>
    ///Это класс теста для DraughtTest, в котором должны
    ///находиться все модульные тесты DraughtTest
    ///</summary>
    [TestClass]
    public class DraughtTest
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
        ///Тест для Конструктор Draught
        ///</summary>
        [TestMethod]
        public void DraughtConstructorTest()
        {
            int x = 4;
            int y = 2;
            DraughtType type = DraughtType.Queen;
            Players player = Players.PlayerTwo;
            Draught target = new Draught(x, y, type, player);

            Assert.AreEqual(x, target.X);
            Assert.AreEqual(y, target.Y);
            Assert.AreEqual(type, target.Type);
            Assert.AreEqual(player, target.Player);
        }

        /// <summary>
        ///Тест для Конструктор Draught
        ///</summary>
        [TestMethod]
        public void DraughtConstructorTest1()
        {
            int x = 3;
            int y = 2;
            Players player = Players.PlayerOne;
            Draught target = new Draught(x, y, player);
            target.Type = DraughtType.Queen;

            Assert.AreEqual(x, target.X);
            Assert.AreEqual(y, target.Y);
            Assert.AreEqual(DraughtType.Queen, target.Type);
            Assert.AreEqual(player, target.Player);
        }

        /// <summary>
        ///Тест для Конструктор Draught
        ///</summary>
        [TestMethod]
        public void DraughtConstructorTest2()
        {
            BoardPoint point = new BoardPoint(2, 4);
            DraughtType type = DraughtType.Queen;
            Players player = Players.PlayerTwo;
            Draught target = new Draught(point, type, player);

            Assert.AreEqual(point, target.Point);
            Assert.AreEqual(type, target.Type);
            Assert.AreEqual(player, target.Player);
        }

    }
}
