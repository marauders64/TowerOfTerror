using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TowerOfTerror.Model
{
    // Enumerated type that determines whether an entity is alive, dead, or falling
    public enum Life { Alive, Dead, Falling };

    // Enumerated type that determines the kind of entity
    public enum Type { Character, Enemy };

    // Contains general information for all living things
    abstract class Entity : ISerializable
    {
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

        // Is the entity falling?
        public abstract bool IsFalling();

        // Returns a string matching the kind of entity it is
        public abstract Type GetKind();

        public abstract List<string> Serialize();

        public abstract void Deserialize(List<Object> savedData);
    }
}
