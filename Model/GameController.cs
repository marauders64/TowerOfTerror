using System;
using System.Collections.Generic;
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
        public List<Level> Floors { get; set; }
        public Difficulty Setting { get; set; }
        public List<Enemy> Enemies { get; set; }
        public bool Cheating { get; set; }
        public Level currentFloor;
        public Character adventurer;

        public static Random rand = new Random();
        
        public GameController(Difficulty setting)
        {
            this.Floors = new List<Level>();
            this.Setting = setting;
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
            List<string> data= new List<string>();
            data.Add("GameController");
            //add reference to which Floor we're on right now
            //add reference to Character... I think...
            data.Add(Setting.ToString());

            return data;
        }

        public void Deserialize(List<object> savedData)
        {
            //get Entity List of save data
            //Loop through and assign each property its corresponding saved value
            throw new NotImplementedException();
        }

        public void Save()
        {
            // create a new file and open it
            //call Serialize and write the result to file
            // call Level's Serialize and write to file
            //using the reference to the Floor as a reference point, loop through all Enemies and call Serialize
            //Character Seralize

        }

        public void Load(string txtFile)
        {
            throw new NotImplementedException();
        }
    }
}
