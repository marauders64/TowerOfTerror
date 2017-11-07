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
        public static int index = 1;
        public int Index { get; set; }
        public int Factor { get; set; }
        public PowerUp Type { get; set; }

        // Drop an item of the specified type
        public Item(PowerUp powerUp)
        {
            this.Index = index;
            index++;
            this.Type = powerUp;
        }

        // By default, drop a healing item
        public Item()
        {
            this.Index = index;
            index++;
            this.Type = PowerUp.Heal;
        }
    }
}