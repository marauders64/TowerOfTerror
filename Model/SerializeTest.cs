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
        //------------------------------------------------
        //--------------- SAVE / SERIALIZE ---------------
        //------------------------------------------------

        [TestMethod]
        public void Character_Serialize_Converted()
        {
            Character c = new Character();
            List<string> test = c.Serialize();
            Assert.IsTrue(test[0] == "Character");
            //Assert.IsTrue(test[1] == "McCoy"); <-- Name comes down from View, which Model knows nothing about
            // test[2] is Image and turned out to be exclusively a View-level thing
            Assert.IsTrue(test[3] == "245");
            Assert.IsTrue(test[4] == "240");
            Assert.IsTrue(test[5] == "25");
            Assert.IsTrue(test[6] == "5");
            Assert.IsTrue(test[7] == "100");
            Assert.IsTrue(test[8] == "Alive");
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
            Level.ResetNum();
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
            gc.Floors[0].FillEnemies(gc.Floors[0]);
            gc.Floors[0].PlaceEnemies();
            string loadedGame; 
            gc.Save(@"ToT.dat");
            using (StreamReader reader = new StreamReader(@"ToT.dat"))
            {
                loadedGame = reader.ReadToEnd();
            }
            string[] gameData = loadedGame.Split(',');
            Assert.IsTrue(gameData[0] == "ToTSave");
            Assert.IsTrue(gameData[1] == "GameController");
            Assert.IsTrue(gameData[5] == "Level");
            Assert.IsTrue(gameData[8] == "Enemy");
            Assert.IsTrue(gameData[17] == "Enemy");
            Assert.IsTrue(gameData[26] == "Character");
        }

        //--------------------------------------------------
        //--------------- LOAD / DESERIALIZE ---------------
        //--------------------------------------------------

        [TestMethod]
        public void GameController_Load_ControllerDeserialized()
        {
            GameController gc = new GameController();
            gc.Load(@"newTestSave.dat");
            Assert.IsTrue(gc.CurrentFloor == 0);
            Assert.IsTrue(gc.Setting == Difficulty.Easy);
        }

        
        [TestMethod]
        public void GameController_Load_LevelDeserialized()
        {
            GameController gc1 = new GameController();
            gc1.BuildTower();
            gc1.Load(@"newTestSave.dat");
            Assert.IsTrue(gc1.Floors[0].Num == 0); 
            Assert.IsTrue(gc1.Floors[0].Type == LevelType.Basic);
        }

        [TestMethod]
        public void GameController_Load_EnemyDeserialized()
        {
            GameController gc = new GameController();
            gc.BuildTower();
            gc.Floors[0].FillEnemies(gc.Floors[0]);
            gc.Floors[0].PlaceEnemies();
            gc.Load(@"newTestSave.dat");
            Assert.IsTrue(gc.Floors[0].Enemies[0].Power == 5);
            Assert.IsTrue(gc.Floors[0].Enemies[0].Defense == 5);
            Assert.IsTrue(gc.Floors[0].Enemies[0].Health == 100);
            Assert.IsTrue(gc.Floors[0].Enemies[0].Status == Life.Alive);
        }

        [TestMethod]
        public void GameController_Load_CharacterDeserialized()
        {
            GameController gc = new GameController();
            gc.BuildTower();
            gc.Load(@"newTestSave.dat");
            //Assert.IsTrue(gc.adventurer.Name == "me");
            //image null right now
            Assert.IsTrue(gc.adventurer.Position.X == 245);
            Assert.IsTrue(gc.adventurer.Position.Y == 240);
            Assert.IsTrue(gc.adventurer.Power == 25);
            Assert.IsTrue(gc.adventurer.Defense == 5);
            Assert.IsTrue(gc.adventurer.Health == 100);
            Assert.IsTrue(gc.adventurer.Status == Life.Alive);
        }
    }

}

