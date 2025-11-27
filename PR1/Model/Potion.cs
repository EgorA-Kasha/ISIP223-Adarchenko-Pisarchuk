using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedRPG;

namespace TextBasedRPG
{
    // зелье восстановления
    public class Potion : IItem
    {
        public string Name => "Зелье исцеления";
    
        public void Equip(Player player) { }
    
        public override string ToString() => Name;
    
        public void Use(Player player)
        {
            player.Health = player.MaxHealth;
            Console.WriteLine("Вы выпили зелье и полностью восстановили здоровье!");
        }
    }
}
