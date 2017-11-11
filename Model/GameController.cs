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
        public List<Entity> Enemies { get; set; }

        public static Random rand = new Random();
        
        public GameController(Difficulty setting)
        {
            this.Setting = setting;
            this.Floors = new List<Level>();
        }

        // Populate the Level list with three levels
        public void Setup()
        {
            throw new NotImplementedException();
        }

        // Does the logic for when the player attacks
        public void PlayerAttack(Entity character, Entity enemy)
        {
            character.Attack(enemy);
        }

        // Does the logic for when the player moves
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
