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
        public Random rand = new Random();
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
        public void FillEnemies(Level lv)
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
            for(int i = 0; i < population; i++)
            {
                Enemies.Add(new Enemy());
            }
        }

        // Fills in the level with all the enemies and a single character
        public void PlaceEnemies()
        {
            rand = new Random();
            foreach (Enemy foe in Enemies)
            {
                int foeX = rand.Next(0, 450);
                int foeY = rand.Next(0, 150);
                foe.Position = new Point(foeX, foeY);
            }
        }

        // Is the level completed (all enemies dead/removed from list)?
        public bool LevelComplete()
        {
            int deadOnes = 0;
            foreach (Enemy en in this.Enemies)
            {
                if (en.Status == Life.Dead)
                {
                    deadOnes++;
                }
            }
            return Enemies.Count == deadOnes;
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
            data.Add("Level");
            data.Add(Num.ToString());
            data.Add(Type.ToString());

            return data;
        }

        /// <summary>
        /// Finds and extracts its own information from the list prepared by Load()
        /// Converts each string to the correct data and updates its state
        /// </summary>
        /// <param name="savedData">Array of string data extracted by Load() from file</param>
        public void Deserialize(string[] savedData)
        {
            Num = Convert.ToInt32(savedData[0]);
            Type = (savedData[1] == "Basic" ? LevelType.Basic : LevelType.Final);
        }
    }
}
