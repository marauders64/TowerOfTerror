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

        // Does not inherit Serializable because this class is merely an object factory
        // Character takes care of Items stored in its Inventory field
    }
}