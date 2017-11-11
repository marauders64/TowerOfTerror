using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfTerror.Model
{
    [TestClass]
    class SerializeTest
    {
        [TestMethod]
        public void Character_Serialize_Converted()
        {
            Character c = new Character();
            List<string> test = new List<string>();
            test = c.Serialize();
            Assert.IsTrue(test[0] == "Character");
            Assert.IsTrue(test[1] == "McCoy");
            // test[2] is Image and is null right now
            Assert.IsTrue(test[3] == "0");
            Assert.IsTrue(test[4] == "0");
            Assert.IsTrue(test[5] == "5");
            Assert.IsTrue(test[6] == "5");
            Assert.IsTrue(test[7] == "100");
            Assert.IsTrue(test[8] == "Alive");
            //Assert.IsTrue(test.Length == however long it ends up) <-- do this later unless you want to keep changing it
        }
    }
}
