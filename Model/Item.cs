using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

/*
 * Item.cs stores the Item class
 * Includes a PowerUp enum for each kind of powerup
 */

namespace TowerOfTerror.Model
{
    // Contains different kinds of powerups
    public enum PowerUp { AtkBuff, DefBuff, Heal };

    // Powerup information
    public class Item 
    {
        // Random seed used for determining item type
        private Random rand;
        // Auto-incremented ID for items
        public static int index = 1;
        // Access the ID
        public int Index { get; set; }
        // Retrieve/store the type of powerup dropped
        public PowerUp Type { get; set; }

        // Instantiate an item
        public Item()
        {
            this.Index = index;
            index++;
        }

        // Randomly generate an item type.
        public PowerUp WhichItem()
        {
            rand = new Random();
            int itemCode = rand.Next(0, 3);
            switch (itemCode) 
            {
                case 0:
                    return PowerUp.AtkBuff;
                case 1:
                    return PowerUp.DefBuff;
                case 2:
                    return PowerUp.Heal;
                default:
                    return PowerUp.Heal;
            }
        }

        // Does not inherit Serializable because this class is merely an object factory
        // Character takes care of Items stored in its Inventory field
    }
}