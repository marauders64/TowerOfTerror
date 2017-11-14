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

    public int CurrentFloor { get; set; }

    // Manages game logic
    class GameController : ISerializable
    {
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
            data.Add(currentFloor.ToString());
            //I don't think I need a reference to Character, like Level didn't reference Enemies
            data.Add(Setting.ToString());

            return data;
        }

        public void Deserialize(List<object> savedData)
        {
            //get Entity List of save data
            //Loop through and assign each property its corresponding saved value
            throw new NotImplementedException();
        }

        /// <summary>
        /// At present this method either creates a new save file or overwrites the old one
        /// defaults to storing the file with the .exe
        /// </summary>
        /// NOTE TO SELF: need to call this method at the end of every Level
        public async void Save()
        {
            string filename = "ToT"; //will eventually let user supply filename
            List<List<string>> allSavedData = new List<List<string>>();

            using (StreamWriter writer = File.CreateText(filename + ".dat"))
            {
                // start the file off with a "header" that will tell Load that this is a valid file
                await writer.WriteAsync("ToTSave");

                // collect all data to be saved
                allSavedData.Add(Serialize()); // list of GameController data
                allSavedData.Add(Floors[CurrentFloor].Serialize());
                foreach (Enemy enemy in Floors[CurrentFloor].Enemies)
                {
                    allSavedData.Add(enemy.Serialize());
                }
                allSavedData.Add(adventurer.Serialize());

                // write to file (comma delimited) asynchronously
                foreach (List<string> classData in allSavedData)
                {
                    foreach (string property in classData)
                    {
                        await writer.WriteAsync("," + property);
                    }
                }
            } // using
        } // Save

        public void Load(string txtFile)
        {
            throw new NotImplementedException();
        }
    }
}
