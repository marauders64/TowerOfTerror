using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Level.cs contains the Level class and 
 * Also contains a LevelType enum
 * Detecting endgame
 * Managing item drops
 * asdf
 */

namespace TowerOfTerror.Model
{
    // Indicates the type of level
    enum LevelType { Basic, Final }

    // Contains data for the levels
    class Level : ISerializable
    {
        // Random seed for enemy generation
        public Random rand = new Random();
        // Auto-incremented ID
        public static int num = 0;
        // Retrieve/store the ID
        public int Num { get; set; }
        // Store the type of Level
        public LevelType Type { get; set; }
        // List of enemies to be contained in each level
        public List<Enemy> Enemies { get; set; }

        // Instantiate a level
        public Level(LevelType type)
        {
            this.Num = num;
            num++;
            this.Type = type;
            this.Enemies = new List<Enemy>();
        }

        /// <summary>
        /// Keeps level numbers correct when creating mutiple game instances
        /// </summary>
        public static void ResetNum()
        {
            num = 0;
        }

        // Determines the amount of enemies to fill each level
        // Only one enemy will appear in the boss level
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
            // Populate the enemy list per level
            if (Type == LevelType.Basic)
            {
                for (int i = 0; i < population; i++)
                {
                    Enemies.Add(new Enemy());
                }
            }
            else // heast: "special case for boss"
            {
                Enemies.Add(new Enemy() {
                    Position = new Point(95, 240),
                    Power = 10,
                    Defense = 10,
                    Health = 200
                });
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
