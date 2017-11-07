﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TowerOfTerror.Model
{
    // Enumerated type that determines whether an entity is alive, dead, or falling.
    public enum Life { Alive, Dead, Falling };

    // Contains general information for all living things
    abstract class Entity
    {
        public int Id { get; }
        public string Image { get; set; }
        public Point Position { get; set; }
        public int Power { get; set; }
        public int Defense { get; set; }
        public int Health { get; set; }
        public Life Status { get; set; }

        // Whoever you are, do an attack
        public abstract void Attack();

        // Is the entity dead? Handle each differently
        public abstract bool IsDead();

        public virtual List<string> Serialize()
        {
            //convert each Property value to a string and stick them all in a List
            List<string> fakeList = new List<string>();
            return fakeList;
        }

        public virtual void Deserialize()
        {
            //get Entity List of save data
            //Loop through and assign each property its saved value
        }
    }
}
