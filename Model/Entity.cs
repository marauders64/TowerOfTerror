using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

/*
 * Entity.cs contains the abstract Entity class that cannot be instantiated
 * Both Enemy and Character inherit from this class, thus sharing all of its attributes
 * Also contains the Life enum
 */

namespace TowerOfTerror.Model
{
    // Enumerated type that tags an entity as Alive or Dead
    public enum Life { Alive, Dead };

    // Contains general information for all living things
    abstract class Entity : ISerializable
    {
        // Appearance of the entity
        public string Image { get; set; }
        // Their position on an x/y plane (used for canvas placing)
        public Point Position { get; set; }
        // Their attack force, used in calculating damage done to an opponent
        public int Power { get; set; }
        // Their defense force, used in mitigating damage done by an opponent
        public int Defense { get; set; }
        // Their life force, determining life or death
        public int Health { get; set; }
        // Retrieves/stores their life status according to the Life enum
        public Life Status { get; set; }
        // Retrieves the direction an entity is facing according to the Direction enum
        public Direction Facing { get; set; }

        // Whoever you are, do an attack
        public abstract void Attack(Entity hitenemy);

        // Is the entity dead? Handle each differently
        public abstract bool IsDead();
        
        // Updates the entity's position
        public abstract void Move(Direction direction);

        // Stringifies the information of an entity (not used)
        public abstract List<string> Serialize();

        // Restores the entity object from a string of data
        public abstract void Deserialize(string[] savedData);
    }
}
