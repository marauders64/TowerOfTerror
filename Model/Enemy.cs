using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerOfTerror.Model;

namespace TowerOfTerror.Model
{

    // Information control for enemies in the game
    class Enemy : Entity
    {
        private static int id = 1;
        public int Id { get; set; }

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

        public override void Attack(Entity hitenemy)
        {
            hitenemy.Health -= (this.Power / hitenemy.Defense) * 10;
        }

        public override Type GetKind()
        {
            throw new NotImplementedException();
        }

        public override bool IsDead()
        {
            throw new NotImplementedException();
        }

        public override bool IsFalling()
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

        public override void Deserialize(List<object> savedData)
        {
            //get Entity List of save data
            //Loop through and assign each property its corresponding saved value
            throw new NotImplementedException();
        }
    }
}
