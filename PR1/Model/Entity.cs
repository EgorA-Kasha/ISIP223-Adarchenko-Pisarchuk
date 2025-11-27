using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedRPG;

namespace TextBasedRPG
{
    // базовый класс сущности
    public class Entity
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public Entity(string name, int health, int attack, int defense)
        {
            Name = name;
            MaxHealth = health;
            Health = health;
            Attack = attack;
            Defense = defense;
        }

        public virtual int TakeDamage(int damage)
        {
            int net = Math.Max(0, damage - Defense);
            Health -= net;
            return net;
        }

        public bool IsAlive => Health > 0;

        public virtual void DisplayStatus()
        {
            double percent = (double)Health / MaxHealth * 100.0;
            ConsoleColor color = percent > 70 ? ConsoleColor.Green : percent > 30 ? ConsoleColor.Yellow : ConsoleColor.Red;
            Console.ForegroundColor = color;
            Console.WriteLine($"{Name}: HP {Health}/{MaxHealth}");
            Console.ResetColor();
        }
    }
}

// сам игрок
public class Player : Entity
{
    public bool IsFrozen { get; set; } = false;
    public Weapon Weapon { get; set; }
    public Armor Armor { get; set; }

    public Player(string name, int health, int attack, int defense) : base(name, health, attack, defense)
    {
        Weapon = null;
        Armor = null;
    }

    public override int TakeDamage(int damage)
    {
        return TakeDamage(damage, false);
    }

    public int TakeDamage(int damage, bool ignoreArmor)
    {
        int totalDef = Defense + (ignoreArmor ? 0 : (Armor?.Defense ?? 0));
        int net = Math.Max(0, damage - totalDef);
        if (net == 0 && damage > 0)
        {
            net = 1;
        }
        Health -= net;
        return net;
    }

    public int GetAttack()
    {
        return Attack + (Weapon?.Attack ?? 0);
    }

    public void Heal(int amount)
    {
        Health += amount;
        if (Health > MaxHealth) Health = MaxHealth;
    }

    public void Unfreeze()
    {
        IsFrozen = false;
    }
}
