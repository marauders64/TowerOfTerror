using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerOfTerror.Model;

/*
 * Enemy.cs stores information for an enemy
 * Each enemy also randomly drops an item
 */

namespace TowerOfTerror.Model
{

    // Information control for enemies in the game
    class Enemy : Entity
    {
        // Random seed used to determine item drops
        Random rand;
        // Each enemy is tagged with an auto-incrementing number
        private static int id = 1;
        // Retrieve an enemy's id
        public int Id { get; set; }

        // Create an enemy
        public Enemy()
        {
            this.Id = id;
            id++;
            this.Image = null; // will eventually be an enemy avatar
            this.Position = new Point(0, 0);
            this.Power = 5;
            this.Defense = 5;
            this.Health = 100;
            this.Status = Life.Alive;
        }

        // Attack the player and decrement their health accordingly
        // Only called if the enemy is close enough to the player
        public override void Attack(Entity hitenemy)
        {
            hitenemy.Health -= (this.Power / hitenemy.Defense) * 10;
        }

        // Detect end of life condition
        public override bool IsDead()
        {
            if(Health <= 0)
            {
                Power = 0; // heast
                Status = Life.Dead; // heast
                return true;
            }
            else
            {
                return false;
            }
        }
        
        // Pick either 0 or 1 and drop an item if 1
        public bool DropsItem()
        {
            rand = new Random();
            int drops = rand.Next(0, 2);
            return drops == 0 ? false : true;
        }

        /// <summary>
        /// Moves the Enemy in the direction it is facing.
        /// </summary>
        /// <param name="dir"></param>
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
        /// Creates and returns a list of stringified state data for this Enemy.
        /// List begins with a header ("Enemy") in index 0, and then serializes each property in turn
        /// </summary>
        /// <returns>list of Enemy state information to be saved</returns>
        public override List<string> Serialize()
        {
            List<string> data = new List<string>();
            data.Add("Enemy"); // "header" info
            data.Add(Id.ToString());
            data.Add(Image);
            data.Add(Position.X.ToString());
            data.Add(Position.Y.ToString());
            data.Add(Power.ToString());
            data.Add(Defense.ToString());
            data.Add(Health.ToString());
            data.Add(Status.ToString());

            return data;
        }

        /// <summary>
        /// Finds and extracts the correct information from the array prepared by Load()
        /// Converts each string to the correct data and updates its state
        /// </summary>
        /// <param name="savedData">Array of string data extracted by Load() from file</param>
        public override void Deserialize(string[] savedData)
        {
            Id = Convert.ToInt32(savedData[0]);
            Image = savedData[1];
            Position = new Point(Convert.ToInt32(savedData[2]), Convert.ToInt32(savedData[3]));
            Power = Convert.ToInt32(savedData[4]);
            Defense = Convert.ToInt32(savedData[5]);
            Health = Convert.ToInt32(savedData[6]);
            string enemyStatus = savedData[7];
            switch (enemyStatus)
            {
                case "Alive":
                    Status = Life.Alive;
                    break;
                case "Dead":
                    Status = Life.Dead;
                    break;
                case "Falling":
                    Status = Life.Falling;
                    break;
            }
        }
    }
}
