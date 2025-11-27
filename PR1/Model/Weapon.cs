using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedRPG;

namespace TextBasedRPG
{
    // оружие
    public class Weapon : IItem
    {
        public string Name { get; }
        public int Attack { get; }
    
        public Weapon(string name, int attack)
        {
            Name = name;
            Attack = attack;
        }
    
        public void Equip(Player player)
        {
            player.Weapon = this;
        }
    
        public override string ToString() => Name;
    }
}
