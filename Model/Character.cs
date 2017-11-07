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
        public string Name { get; set; }
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

        public override void Attack()
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

        public override Type GetKind()
        {
            throw new NotImplementedException();
        }

        public void UseItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
