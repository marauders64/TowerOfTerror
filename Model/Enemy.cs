using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public override void Attack()
        {
            throw new NotImplementedException();
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

        public override List<string> Serialize()
        {
            //convert each Property value to a string and stick them all in a List
            List<string> fakeList = new List<string>();
            return fakeList;
        }

        public override void Deserialize(List<object> savedData)
        {
            //get Entity List of save data
            //Loop through and assign each property its corresponding saved value
            throw new NotImplementedException();
        }
    }
}
