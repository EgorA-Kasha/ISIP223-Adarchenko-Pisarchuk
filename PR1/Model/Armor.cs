using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedRPG
{
    // броня
    public class Armor : IItem
    {
        public string Name { get; }
        public int Defense { get; }

        public Armor(string name, int defense)
        {
            Name = name;
            Defense = defense;
        }

        public void Equip(Player player)
        {
            player.Armor = this;
        }

        public override string ToString() => Name;
    }
}
