using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfTerror.Model
{
    class Character : Entity
    {
        // Stores all items the character can have
        // Add auto-collection for item drops
        public List<Item> inventory;
        public string Name { get; set; }
        public Direction Facing { get; set; }

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
        public override void Attack(Entity hitenemy)
        {
            hitenemy.Health -= (this.Power / hitenemy.Defense) * 10;
        }

        // Returns true if Life is dead
        public override bool IsDead()
        {
            if (Health <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Returns true if Life is falling
        public override bool IsFalling()
        {
            throw new NotImplementedException();
        }

        // Returns type character
        public override Type GetKind()
        {
            throw new NotImplementedException();
        }

        // Let the character use an item of their choice
        public void UseItem(int ind)
        {
            throw new NotImplementedException();
        }

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
                data.Add("items"); 

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
                case "Falling":
                    Status = Life.Falling;
                    break;
            //items not implemented
            }
        }
    }
}
