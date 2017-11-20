using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfTerror.Model
{
    [TestClass]
    public class SerializeTest
    {
        [TestMethod]
        public void Character_Serialize_Converted()
        {
            Character c = new Character();
            List<string> test = c.Serialize();
            Assert.IsTrue(test[0] == "Character");
            //Assert.IsTrue(test[1] == "McCoy"); <-- Name comes down from View, which Model knows nothing about
            // test[2] is Image and is null right now
            Assert.IsTrue(test[3] == "0");
            Assert.IsTrue(test[4] == "0");
            Assert.IsTrue(test[5] == "5");
            Assert.IsTrue(test[6] == "5");
            Assert.IsTrue(test[7] == "100");
            Assert.IsTrue(test[8] == "Alive");
            //Assert.IsTrue(test.Length == however long it ends up) <-- do this later unless you want to keep changing it
        }

        [TestMethod]
        public void Enemy_Serialize_Converted()
        {
            Enemy e = new Enemy();
            List<string> test = e.Serialize();
            Assert.IsTrue(test[0] == "Enemy");
            //Assert.IsTrue(test[1] == "1"); <-- i don't know how many Enemies have been made to this point
            //test[2] is a null image at present
            Assert.IsTrue(test[3] == "0");
            Assert.IsTrue(test[4] == "0");
            Assert.IsTrue(test[5] == "5");
            Assert.IsTrue(test[6] == "5");
            Assert.IsTrue(test[7] == "100");
            Assert.IsTrue(test[8] == "Alive");
        }

        [TestMethod]
        public void Level_Serialize_Converted()
        {
            Level l = new Level(LevelType.Basic);
            List<string> test = l.Serialize();
            Assert.IsTrue(test[0] == "Level");
            Assert.IsTrue(test[1] == "0");
            Assert.IsTrue(test[2] == "Basic");
        }

        [TestMethod]
        public void GameController_Serialize_Converted()
        {
            GameController gc = new GameController();
            gc.Setting = Difficulty.Easy;
            List<string> test = gc.Serialize();
            Assert.IsTrue(test[0] == "GameController");
            Assert.IsTrue(test[1] == "0");
            Assert.IsTrue(test[2] == "Easy");
        }

        [TestMethod]
        public void GameController_Save_Success()
        {
            GameController gc = new GameController();
            gc.BuildTower();
            string loadedGame; 
            gc.Save(@"ToT.dat");
            using (StreamReader reader = new StreamReader(@"ToT.dat"))
            {
                loadedGame = reader.ReadToEnd();
            }
            string[] gameData = loadedGame.Split(',');
            Assert.IsTrue(gameData[0] == "ToTSave");
            Assert.IsTrue(gameData[1] == "GameController");
            Assert.IsTrue(gameData[4] == "Level");
            Assert.IsTrue(gameData[7] == "Enemy");
            Assert.IsTrue(gameData.Length == 35);
        }

        /// <summary>
        /// Setup method for Load/Deserialize tests
        /// need to fix to relative path
        /// </summary>
       /* public void Deserialize_Setup()
        {
            GameController gc = new GameController();
            string loadedGame;
            gc.Save();
            ///gc.Load();
        }*/


       /* [TestMethod]
        public void GameController_Load_Success()
        {
            GameController gc = new GameController();
            string loadedGame;
            gc.Save();
            gc.Load(@"C:\Users\Heather\Desktop\Fall2017\CpS209\programs\TowerOfTerror\SavedGames\ToT.dat");
            //will have to assert stuff about deserialize...
        }

        [TestMethod]
        public void GameController_Deserialize_Set()
        {
            Deserialize_Setup();

        }*/
    }
}

