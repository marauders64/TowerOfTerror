using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TowerOfTerror.Model
{
    public enum PowerUp { AtkBuff, DefBuff, Heal };
    public class Item 
    {
        private Random rand;
        public static int index = 1;
        public int Index { get; set; }
        public PowerUp Type { get; set; }

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