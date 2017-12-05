using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Character.cs contains the character class
 * Character-specific save/load logic is also contained in this file
 */

namespace TowerOfTerror.Model
{
    // Contains information for the player-controlled character
    class Character : Entity
    {
        // Stores all items the character can have (items are autocollected)
        public List<Item> inventory;

        // Stores/retrieves the character's name as entered into a text box
        // Used for high scores
        public string Name { get; set; }

        // Create a character
        public Character()
        {
            this.Image = null; // will eventually be our character avatar
            this.Position = new Point(245, 240);
            this.Power = 25;
            this.Defense = 5;
            this.Health = 100;
            this.Status = Life.Alive;
            this.inventory = new List<Item>();
            this.Facing = Direction.Up;
        }

        // Attack the enemy and reduce enemy health
        // This method is only invoked if the character is close enough to do damage
        public override void Attack(Entity hitenemy)
        {
            hitenemy.Health -= (this.Power / hitenemy.Defense) * 10;
        }

        // Returns true if health is depleted
        public override bool IsDead()
        {
            return Health <= 0;
        }
        
        // Updates the Character's position
        public override void Move(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    this.Position = new Point(Position.X, Position.Y - 2);
                    break;
                case Direction.Down:
                    this.Position = new Point(Position.X, Position.Y + 2);
                    break;
                case Direction.Right:
                    this.Position = new Point(Position.X + 2, Position.Y);
                    break;
                case Direction.Left:
                    this.Position = new Point(Position.X - 2, Position.Y);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Creates and returns a list of stringified state data for this Character.
        /// List begins with a "character" header in index 0, and then serializes each property in turn
        /// </summary>
        /// <returns>list of Character state information to be saved</returns>
        public override List<string> Serialize()
        {
            List<string> data = new List<string>();
            data.Add("Character"); // "header" info
            data.Add(Name);
            data.Add(Image); 
            data.Add(Position.X.ToString());
            data.Add(Position.Y.ToString());
            data.Add(Power.ToString());
            data.Add(Defense.ToString());
            data.Add(Health.ToString());
            data.Add(Status.ToString());
            if (inventory != null)
            {
                data.Add("Items"); 

                foreach (Item item in inventory)
                {
                    data.Add(item.Index.ToString());
                    data.Add(item.Type.ToString());
                }
            }

            return data;
        }

        /// <summary>
        /// Finds and extracts the correct information from the array prepared by Load()
        /// Converts each string to the correct data and updates its state
        /// </summary>
        /// <param name="savedData">Array of string data extracted by Load() from file</param>
        public override void Deserialize(string[] savedData)
        {
            Name = savedData[0];
            Image = savedData[1];
            Position = new Point(Convert.ToInt32(savedData[2]), Convert.ToInt32(savedData[3]));
            Power = Convert.ToInt32(savedData[4]);
            Defense = Convert.ToInt32(savedData[5]);
            Health = Convert.ToInt32(savedData[6]);

            string heroStatus = savedData[7];
            switch (heroStatus)
            {
                case "Alive":
                    Status = Life.Alive;
                    break;
                case "Dead": // i don't know why anyone would save after dying, though...
                    Status = Life.Dead;
                    break;
                
            }

            int itemIndex = Array.IndexOf(savedData, "Items");
            if (itemIndex < savedData.Length)
            {
                for (int i = itemIndex + 1; i < savedData.Length; i += 2)
                {
                    int index = Convert.ToInt32(savedData[i]);
                    string itemType = savedData[i + 1];
                    switch (itemType)
                    {
                        case "AtkBuff":
                            inventory.Add(new Item() { Index = index, Type = PowerUp.AtkBuff });
                            break;
                        case "DefBuff":
                            inventory.Add(new Item() { Index = index, Type = PowerUp.DefBuff });
                            break;
                        case "Heal":
                            inventory.Add(new Item() { Index = index, Type = PowerUp.Heal });
                            break;
                    }
                }
            }


        }
    }
}
