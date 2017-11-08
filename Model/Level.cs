using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfTerror.Model
{
    // Indicates the type of level
    enum LevelType { Basic, Final }

    // Contains data for the levels
    class Level : ISerializable
    {
        public static int num = 0;
        public int Num { get; set; }
        public LevelType Type { get; set; }
        public List<Enemy> Enemies { get; set; }

        public Level(LevelType type)
        {
            this.Num = num;
            num++;
            this.Type = type;
            this.Enemies = new List<Enemy>();
        }

        // Determines the amount of enemies to fill each level
        public int FillEnemies()
        {
            return this.Num;
        }

        // Fills in the level with all the enemies and a single character
        public void PlaceEntities()
        {
            throw new NotImplementedException();
        }

        // Is the level completed (all enemies dead)?
        public bool LevelComplete()
        {
            return false;
        }

        // only needed if Player saves game mid-level
        public List<string> Serialize()
        {
            //convert each Property value to a string and stick them all in a List
            List<string> fakeList = new List<string>();
            return fakeList;
        }

        public void Deserialize(List<object> savedData)
        {
            //get Entity List of save data
            //Loop through and assign each property its corresponding saved value
            throw new NotImplementedException();
        }
    }
}
