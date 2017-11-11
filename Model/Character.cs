using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfTerror.Model
{
    class Character: Entity
    {
        // Stores all items the character can have
        // Add auto-collection for item drops
        public List<Item> inventory;
        public string Name { get; set; }

        // Name a character with the provided name, or default it to McCoy
        public Character(string name)
        {
            this.Name = name;
            this.Image = null; // will eventually be our character avatar
            this.Position = new Point(0, 0);
            this.Power = 5;
            this.Defense = 5;
            this.Health = 100;
            this.Status = Life.Alive;
        }
        public Character()
        {
            this.Name = "McCoy";
            this.Image = null; // will eventually be our character avatar
            this.Position = new Point(0, 0);
            this.Power = 5;
            this.Defense = 5;
            this.Health = 100;
            this.Status = Life.Alive;
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
                    this.Position = new Point(Position.X, Position.Y + 10);
                    break;
                case Direction.Down:
                    this.Position = new Point(Position.X, Position.Y - 10);
                    break;
                case Direction.Right:
                    this.Position = new Point(Position.X + 10, Position.Y);
                    break;
                case Direction.Left:
                    this.Position = new Point(Position.X - 10, Position.Y);
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
            data.Add(Image); // should already be a string: image name or a reference to its location
            data.Add(Position.X.ToString());
            data.Add(Position.Y.ToString());
            data.Add(Power.ToString());
            data.Add(Defense.ToString());
            data.Add(Health.ToString());
            data.Add(Status.ToString());
            data.Add("items"); // "sub-header" that may go away; for now will eventually let Deserialize know that a list needs to be reconstructed
            
            foreach (Item item in inventory)
            {
                data.Add(item.ToString());
            }
            
            return data;
        }

        public override void Deserialize(List<object> savedData)
        {
            //get Entity List of save data
            //Loop through and assign each property its corresponding saved value
            throw new NotImplementedException();
        }
    }
}
