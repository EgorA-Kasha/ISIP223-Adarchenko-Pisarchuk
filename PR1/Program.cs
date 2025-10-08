using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

// базовый класс для сущностей (игрок и враги)
abstract class Entity
{
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }

    protected Entity(int hp, int attack, int defense)
    {
        MaxHP = hp;
        HP = hp;
        Attack = attack;
        Defense = defense;
    }

    // абстрактный метод атаки
    public abstract void AttackTarget(Entity target, Random rand);
}

// интерфейс для предметов
interface IItem
{
    string Name { get; }
    void Equip(Player player);
}

// оружие
class Weapon : IItem
{
    public string Name { get; }
    public int AttackBonus { get; }

    public Weapon(string name, int attackBonus)
    {
        Name = name;
        AttackBonus = attackBonus;
    }

    public void Equip(Player player)
    {
        player.Weapon = this;
        player.Attack = 5 + AttackBonus; // базовая атака + бонус
    }

    public override string ToString() => $"{Name} (Атака: {AttackBonus})";
}

// доспехи
class Armor : IItem
{
    public string Name { get; }
    public int DefenseBonus { get; }

    public Armor(string name, int defenseBonus)
    {
        Name = name;
        DefenseBonus = defenseBonus;
    }

    public void Equip(Player player)
    {
        player.Armor = this;
        player.Defense = 2 + DefenseBonus; // базовая защита + бонус
    }

    public override string ToString() => $"{Name} (Защита: {DefenseBonus})";
}

// зелье исцеления
class Potion
{
    public string Name => "Зелье исцеления";

    public void Use(Player player)
    {
        player.HP = player.MaxHP;
        Console.WriteLine("Зелье исцеления! HP восстановлено!");
    }

    public override string ToString() => Name;
}

// игрок
class Player : Entity
{
    public Weapon Weapon { get; set; }
    public Armor Armor { get; set; }
    public bool IsFrozen { get; set; }

    public Player() : base(100, 8, 3) // базовые значения
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

    // защита
    public int Defend(Random rand)
    {
        if (rand.NextDouble() < 0.4)
        {
            Console.WriteLine("Вы уклонились от атаки!");
            return -1; // уклонение
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

// враг
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

// Фабрика для генерации врагов и боссов
class EnemyFactory
{
    private Random rand = new Random();

    public Enemy GenerateEnemy()
    {
        int type = rand.Next(3);
        switch (type)
        {
            case 0: return new Enemy("Гоблин", 30, 8, 2, 0.2, 0, false);
            case 1: return new Enemy("Скелет", 40, 7, 3, 0, 0, true);
            case 2: return new Enemy("Маг", 35, 9, 1, 0, 0.15, false);
            default: return null;
        }
    }

    public Enemy GenerateBoss()
    {
        int type = rand.Next(4);
        switch (type)
        {
            case 0: return new Enemy("ВВГ", (int)(50 * 2.0), (int)(8 * 1.5), (int)(2 * 1.2), 0.2 + 0.1, 0, false);
            case 1: return new Enemy("Ковальский", (int)(60 * 2.5), (int)(7 * 1.3), (int)(3 * 1.4), 0, 0, true);
            case 2: return new Enemy("Архимаг C++", (int)(40 * 1.8), (int)(9 * 1.6), (int)(1 * 1.1), 0, 0.15 + 0.1, false);
            case 3: return new Enemy("Пестов С--", (int)(60 * 1.3), (int)(7 * 1.8), (int)(3 * 0.6), 0, 0.15 + 0.15, true);
            default: return null;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {

    }
}