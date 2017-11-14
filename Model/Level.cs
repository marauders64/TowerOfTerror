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
        public Character dude;
        public Random rand;
        public static int num = 0;
        public int Num { get; set; }
        public LevelType Type { get; set; }
        public List<Enemy> Enemies { get; set; }

        public Level(LevelType type)
        {
            this.Num = num;
            num++;
            this.Type = type;
            this.dude = new Character();
            this.Enemies = new List<Enemy>();
        }

        // Determines the amount of enemies to fill each level
        public int FillEnemies(Level lv)
        {
            int population;
            if (lv.Type == LevelType.Final)
            {
                population = 1;
            }
            else
            {
                switch (lv.Num)
                {
                    case 1:
                        population = 2;
                        break;
                    case 2:
                        population = 3;
                        break;
                    case 3:
                        population = 5;
                        break;
                    default:
                        population = 2;
                        break;
                }
            }
            return population;
        }

        // Fills in the level with all the enemies and a single character
        public void PlaceEntities(Level lv)
        {
            
        }

        // Is the level completed (all enemies dead/removed from list)?
        public bool LevelComplete()
        {
            return Enemies.Count == 0;
        }

        /// <summary>
        /// Creates and returns a list of stringified state data for this Level (which provides access to Enemies, etc.).
        /// List begins with a header ("Level") in index 0, and then serializes each property in turn
        /// Note: enemy information is omitted--saving Enemy information is GameController's job
        /// </summary>
        /// <returns>list of level information to be saved</returns>
        public List<string> Serialize()
        {
            List<string> data = new List<string>();
            data.Add("Level"); // "header" info
            data.Add(Num.ToString());
            data.Add(Type.ToString());

            return data;
        }

        public void Deserialize(List<object> savedData)
        {
            //get Entity List of save data
            //Loop through and assign each property its corresponding saved value
            throw new NotImplementedException();
        }
    }
}
