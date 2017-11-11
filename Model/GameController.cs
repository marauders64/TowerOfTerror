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

    // Manages game logic
    class GameController : ISerializable
    {
        public List<Level> Floors { get; set; }
        public Difficulty Setting { get; set; }
        
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

        }

        public void Load(string txtFile)
        {

        }
    }
}
