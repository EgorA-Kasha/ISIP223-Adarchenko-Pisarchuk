using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedRPG;

namespace TextBasedRPG
{
    // Интерфейс для предметов
    public interface IItem
    {
        string Name { get; }
        void Equip(Player player);
    }
}
