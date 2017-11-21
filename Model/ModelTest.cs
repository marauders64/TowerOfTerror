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
    public class ModelTest
    {
        // Oh hi Mark
        GameController gc = new GameController();
        
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
            GameController gc1 = new GameController();
            gc1.Setting = Difficulty.Easy;
            Assert.IsTrue(gc1.adventurer.Health == 100);
        }

        /*[TestMethod]
        public void GameController_Constructor_DifficultyMediumSet()
        {
            GameController gc1 = new GameController();
            gc1.Setting = Difficulty.Medium;
            Assert.IsTrue(gc1.adventurer.Health == 80);
        }

        [TestMethod]
        public void GameController_Constructor_DifficultyHardSet()
        {
            GameController gc1 = new GameController();
            gc1.Setting = Difficulty.Hard;
            Assert.IsTrue(gc1.adventurer.Health == 60);
        }*/

        

    }
}
