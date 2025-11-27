using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedRPG;

namespace TextBasedRPG
{
    // Абстрактный класс врага
    public abstract class AbstractEnemy : Entity
    {
        public bool IsBoss { get; set; }
        public double Resistance { get; set; } = 0;
        public bool IgnoresArmor { get; set; } = false;
    
        public AbstractEnemy(string name, int health, int attack, int defense, double resistance = 0, bool isBoss = false, bool ignoresArmor = false) : base(name, health, attack, defense)
        {
            Resistance = resistance;
            IsBoss = isBoss;
            IgnoresArmor = ignoresArmor;
        }
    
        public virtual void AttackWithDefend(Entity target, int playerDefend = 0)
        {
            int gross = Attack;
    
            // Критический удар для Goblin и VVG
            if (this is Goblin || this is VVG)
            {
                double critChance = this is Goblin ? 0.2 : 0.3;
                bool crit = RandomHelper.NextDouble() < critChance;
                if (crit)
                {
                    gross *= 2;
                    Console.WriteLine("Критический удар!");
                }
            }
    
            // Защита игрока: уклонение или блок
            if (playerDefend == 1)
            {
                bool dodge = RandomHelper.NextDouble() < 0.4;
                if (dodge)
                {
                    Console.WriteLine("Вы уклонились от атаки!");
                    gross = 0;
                }
                else
                {
                    int totalDef = (target as Player).Defense + ((target as Player).Armor?.Defense ?? 0);
                    if (IgnoresArmor) totalDef = (target as Player).Defense;
                    int blockPercent = RandomHelper.Next(70, 101);
                    int blockReduction = (int)(totalDef * blockPercent / 100.0);
                    gross -= blockReduction;
                    gross = Math.Max(0, gross);
                    Console.WriteLine($"Блок сработал! Урон снижен на {blockReduction}.");
                }
            }
    
            // Нанесение урона, если gross > 0 или игнор брони
            if (gross > 0 || IgnoresArmor)
            {
                int net;
                if (IgnoresArmor)
                {
                    Console.WriteLine($"{Name} игнорирует вашу броню!");
                    net = (target as Player)?.TakeDamage(gross, true) ?? target.TakeDamage(gross);
                }
                else
                {
                    net = target.TakeDamage(gross);
                }
                Console.WriteLine($"{Name} наносит {net} урона!");
            }
    
            // Freeze для Mag и Archimag
            if ((this is Mag || this is ArchimagCpp) && Health > 0 && RandomHelper.NextDouble() < 0.3)
            {
                var player = target as Player;
                if (player != null)
                {
                    player.IsFrozen = true;
                    Console.WriteLine($"{Name} замораживает вас! Следующий ход пропущен.");
                }
            }
        }
    }
}
