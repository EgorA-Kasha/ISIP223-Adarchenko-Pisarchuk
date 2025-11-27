using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedRPG
{
    public class Goblin : AbstractEnemy
    {
        public Goblin() : base("Гоблин", 35, 5, 2) { }
    }

    public class Mag : AbstractEnemy
    {
        public Mag() : base("Маг", 40, 6, 1) { }
    }
    public class Skeleton : AbstractEnemy
    {
        public Skeleton() : base("Скелет", 50, 7, 2, 0, false, true) { }
    }
    public class Slime : AbstractEnemy
    {
        public Slime() : base("Слизень", 45, 6, 2) { }

        public override int TakeDamage(int damage)
        {
            Console.WriteLine("Слизень уменьшает урон на 2!");
            int net = Math.Max(0, damage - Defense - 2);
            Health -= net;
            return net;
        }
    }
}