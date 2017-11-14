using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TowerOfTerror.Model
{
    [TestClass]
    class ModelTest
    {
        GameController gc = new GameController(Difficulty.Easy);
        
        [TestMethod]
        public void Attack_EnemyTakesDamage()
        {
            Character hero = new Character();
            Enemy villain = new Enemy();
            hero.Attack(villain);
            AttackResult result = AttackResult.Hit;
            Assert.IsTrue(villain.Health == 90);
        }
        [TestMethod]
        public void GameController_Constructor_DifficultyEasySet()
        {
            GameController gc1 = new GameController(Difficulty.Easy);
            Assert.IsTrue(gc1.adventurer.Health == 100);
        }

        [TestMethod]
        public void GameController_Constructor_DifficultyMediumSet()
        {
            GameController gc1 = new GameController(Difficulty.Medium);
            Assert.IsTrue(gc1.adventurer.Health == 80);
        }

        [TestMethod]
        public void GameController_Constructor_DifficultyHardSet()
        {
            GameController gc1 = new GameController(Difficulty.Hard);
            Assert.IsTrue(gc1.adventurer.Health == 60);
        }

        

    }
}
