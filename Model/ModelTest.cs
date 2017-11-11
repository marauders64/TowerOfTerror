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


    }
}
