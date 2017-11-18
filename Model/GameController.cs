using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfTerror.Model
{
    // Indicates whether the entity was hit, missed, killed, or dropped
    public enum AttackResult { Hit, Miss, Kill, Drop }

    // Sets the difficulty of the level
    public enum Difficulty { Easy, Medium, Hard }

    // Gives the direction an entity is facing or moving
    public enum Direction { Up, Down, Left, Right }


    // Manages game logic
    class GameController : ISerializable
    {
        public int CurrentFloor { get; set; }
        public List<Level> Floors { get; set; }
        public Difficulty Setting { get; set; }
        public List<Enemy> Enemies { get; set; }
        public bool Cheating { get; set; }
        public Level currentFloor;
        public Character adventurer;

        public static Random rand = new Random();
        
        public GameController()
        {
            this.Floors = new List<Level>();
            this.Setting = Difficulty.Easy;
            this.adventurer = new Character();
        }

        public void BuildTower()
        {
            this.Floors.Add(new Level(LevelType.Basic));
            this.Floors.Add(new Level(LevelType.Basic));
            this.Floors.Add(new Level(LevelType.Basic));
        }

        // Populate the Level list with three levels
        public void Setup()
        {
            // Adjust character health according to difficulty
            if (!this.Cheating) {
                switch (this.Setting)
                {
                    case Difficulty.Easy:
                        this.adventurer.Health = 100;
                        break;
                    case Difficulty.Medium:
                        this.adventurer.Health = 80;
                        break;
                    case Difficulty.Hard:
                        this.adventurer.Health = 60;
                        break;
                    default:
                        this.adventurer.Health = 100;
                        break;
                }
            }
            this.currentFloor = this.Floors[0]; // for now just load the main level
            foreach (Level lev in this.Floors)
            {
                lev.FillEnemies(lev);
                lev.PlaceEntities();
            }
            //currentFloor.PlaceEntities();
            //currentFloor.FillEnemies(currentFloor);
            //Enemies = currentFloor.Enemies;

            //Save() call for testing purposes
            Save();
        }

        // Does the logic for when the player attacks
        public void PlayerAttack(Entity character, Entity enemy)
        {
            character.Attack(enemy);
        }

        // Does the logic for when the player moves
        // Moves the enemies in a random direction
        public void UpdatePositions(Character character, Direction direction)
        {
            character.Move(direction);
            foreach (Entity enemy in Enemies)
            {
                //Need to Fix
                int i = rand.Next(1, 5);
                Direction dir;
                switch(i)
                {
                    case 1:
                        dir = Direction.Up;
                        break;
                    case 2:
                        dir = Direction.Down;
                        break;
                    case 3:
                        dir = Direction.Left;
                        break;
                    case 4:
                        dir = Direction.Right;
                        break;
                    default:
                        dir = Direction.Up;
                        break;
                }
                enemy.Move(dir);
            }
        }

        // Returns whether or not the game has ended (player completes final level or dies)
        public bool IsGameOver()
        {
            return false;
        }

        // Records the result of an attack and does the corresponding logic
        public AttackResult AttackResult()
        {
            return Model.AttackResult.Hit;
        }

        /// <summary>
        /// Creates and returns a list of stringified state data for this GameController.
        /// List begins with a header ("GameController") in index 0, and then serializes each property in turn
        /// </summary>
        /// <returns>list of game state information to be saved</returns>
        public List<string> Serialize()
        {
            List<string> data = new List<string>();
            data.Add("GameController");
            data.Add(CurrentFloor.ToString());
            data.Add(Setting.ToString());
            //data.Add(Convert.ToInt32(Setting).ToString()); 

            return data;
        }

        /// <summary>
        /// Finds and extracts its own information from the list prepared by Load()
        /// Converts each string to the correct data and updates its state
        /// </summary>
        /// <param name="savedData">Array of string data extracted by Load() from file</param>
        public void Deserialize(string[] savedData)
        {
            int i = Array.IndexOf(savedData, "GameController");
            CurrentFloor = Convert.ToInt32(savedData[i + 1]);  // <-- VERY FRAGILE, need to confirm exactly how leveling is going to be handled
            //int difficulty = Convert.ToInt32(savedData[i + 2]);
            string difficulty = savedData[i + 2];
            switch (difficulty)
            {
                case "Easy":
                    Setting = Difficulty.Easy;
                    break;
                case "Medium":
                    Setting = Difficulty.Medium;
                    break;
                case "Hard":
                    Setting = Difficulty.Hard;
                    break;
            }
        }

        /// <summary>
        /// At present this method either creates a new save file or overwrites the old one
        /// defaults to storing the file with the .exe
        /// </summary>
        /// NOTE TO SELF: need to add save button when gameplay is functional
        public async void Save()
        {
            string filename = "ToT"; //will eventually let user supply filename
            string path = @"SavedGames\ToT.dat";

            List<List<string>> allSavedData = new List<List<string>>();

            using (StreamWriter writer = new StreamWriter(path))  
            {
                // start the file off with a "header" that will tell Load that this is a valid file
                await writer.WriteAsync("ToTSave");

                // collect all data to be saved
                allSavedData.Add(Serialize()); 
                allSavedData.Add(Floors[CurrentFloor].Serialize());
                foreach (Enemy enemy in Floors[CurrentFloor].Enemies)
                {
                    allSavedData.Add(enemy.Serialize());
                }
                allSavedData.Add(adventurer.Serialize());

                // write to file 
                foreach (List<string> classData in allSavedData)
                {
                    foreach (string property in classData)
                    {
                        await writer.WriteAsync("," + property);
                    }
                }
            } // using
        } // Save

        // will need to start a new game, but update the game state before actually launching it
        // TODO: exception handling
        // TODO: get user input going, get rid of test input
        public async void Load(string dotDat)
        {
            string file;
            
            // are the path and file names correct?
            try
            {
                using (StreamReader reader = new StreamReader(dotDat))
                {
                    file = await reader.ReadToEndAsync();
                }
                // is this file a valid saved ToT game?
                string[] gameData = file.Split(',');

                if (gameData[0] == "ToTSave")
                {
                    // start the game here, i think
                    Deserialize(gameData);
                    currentFloor.Deserialize(gameData);
                    foreach (Enemy enemy in currentFloor.Enemies)
                    {
                        enemy.Deserialize(gameData);
                    }
                    adventurer.Deserialize(gameData);
                }
                else
                {
                    //is being read line by line after the try block... but is not throwing
                    throw new FileFormatException();
                }
            }
            catch
            {
                throw new FileNotFoundException(); // is not being caught...
            }


        }
    }
}
