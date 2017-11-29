using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TowerOfTerror.Model
{
    public enum PowerUp { AtkBuff, DefBuff, Heal };
    public class Item : ISerializable
    {
        private Random rand;
        public static int index = 1;
        public int Index { get; set; }
        public PowerUp Type { get; set; }

        public Item()
        {
            this.Index = index;
            index++;
            rand = new Random();
        }

        // Randomly generate an item type.
        public PowerUp WhichItem()
        {
            int itemCode = rand.Next();
            switch (itemCode)
            {
                case 0:
                    return PowerUp.Heal;
                case 1:
                    return PowerUp.AtkBuff;
                case 2:
                    return PowerUp.DefBuff;
                default:
                    return PowerUp.Heal;
            }
        }


        /// <summary>
        /// Creates and returns a list of stringified state data for this GameController.
        /// List begins with a header ("GameController") in index 0, and then serializes each property in turn
        /// </summary>
        /// <returns>list of game state information to be saved</returns>
        public List<string> Serialize()
        {
            //don't implement until we get to the Save btn stage
            //right now all the items will be accounted for in Character's Item list
            List<string> fakeList = new List<string>();
            return fakeList;
        }

        public void Deserialize(string[] savedData)
        {
            //get Entity List of save data
            //Loop through and assign each property its corresponding saved value
        }
    }
}