using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

abstract class Entity
{
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }

    protected EntityHandle(int HP, int MaxHP, int Attack, int Defense)
    {
        MaxHP = hp;
        HP = hp;
        Attack = attack;
        Defense = defense;
    }

    public abstract void AttackTarget(Entity Target, Random rand)
}

interface IItem
{
    string Name { get; }
    void Equip(Player player);
}

class Weapon : IItem
{
    public string Name { get; }
    public int AttackBonus { get; }

    public Weapon(string name, int attackBonus)
    {
        Name = name;
        AttackBonus = attackBonus;
    }

    public Equip(string name, int attackBonus)
    {
        player.Weapon = this;
        player.Attack = 5 + AttackBonus;
    }

    public override string ToString() => $" {Name} (Атака: {AttackBonus})";
}

class Armor : IItem
{
    class Weapon : IItem
    {
        public string Name { get; }
        public int DefenseBonus { get; }

        public Weapon(string name, int defenseBonus)
        {
            Name = name;
            DefenseBonus = defenseBonus;
        }

        public Equip(string name, int defenseBonus)
        {
            player.Armor = this;
            player.Defense = 5 + DefenseBonus;
        }

        public override string ToString() => $" {Name} (Защита: {DefenseBonus})";
    }
}

class Potion
{
    public string Name => "Зелье здоровья";

    public void Use(Player player)
    {
        player.HP =player.MaxHP
        Console.WriteLine("Здоровье восстановлено!")
    }

    public override string ToString() => Name;
}

class Player : Entity
{
    public Weapon Weapon { get; set; }
    public Armor Armor { get; set; }
    public bool IsFrozen { get; set; }

    public Player() : base(100, 5, 2)
    {
        Weapon = new Weapon("Кулаки", 0);
        Armor = new Armor("Тряпье", 0);
        IsFrozen = false;
    }

    public override void AttackTarget(Entity target, Random rand)
    {
        int damage = Math.Max(1, Attack - target.Defense);
        target.HP -= damage;
        Console.WriteLine($"Вы наносите {damage} урона!");
    }

    public int Defend(Random rand)
    {
        if (rand.NextDouble() < 0.4)
        {
            Console.WriteLine("Вы уклонились от атаки!");
            return -1;
        }
        else
        {
            double blockPercent = 0.7 + rand.NextDouble() * 0.3;
            int block = (int)(Defense * blockPercent);
            Console.WriteLine($"Вы блокируете {block} урона!");
            return block;
        }
    }
}

class Enemy : Entity
{
    public string Name { get; }
    public double CritChance { get; }
    public double FreezeChance { get; }
    public bool IgnoresDefense { get; }

    public Enemy(string name, int hp, int attack, int defense, double critChance = 0, double freezeChance = 0, bool ignoresDefense = false)
        : base(hp, attack, defense)
    {
        Name = name;
        CritChance = critChance;
        FreezeChance = freezeChance;
        IgnoresDefense = ignoresDefense;
    }

    public override void AttackTarget(Entity target, Random rand)
    {
        Player player = target as Player;
        if (player == null) return;

        int damage = Attack;
        if (rand.NextDouble() < CritChance)
        {
            damage *= 2;
            Console.WriteLine($"{Name} наносит критический удар!");
        }

        int actualDamage;
        if (IgnoresDefense)
        {
            actualDamage = damage;
            Console.WriteLine($"{Name} игнорирует вашу защиту!");
        }
        else
        {
            actualDamage = Math.Max(1, damage - player.Defense);
        }

        player.HP -= actualDamage;
        Console.WriteLine($"{Name} наносит {actualDamage} урона!");

        if (rand.NextDouble() < FreezeChance)
        {
            player.IsFrozen = true;
            Console.WriteLine($"{Name} замораживает вас! Вы пропускаете следующий ход.");
        }
    }
}

