using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    abstract class Entity
{
    // базовый класс для сущностей (игрок и враги)
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

        public void DisplayHP(string label)
        {
            Console.Write($"{label}: ");
            Console.ForegroundColor = ConsoleColor.White;
            if (HP <= 0.30 * MaxHP)
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(HP);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"/{MaxHP}");
        }
    }

// игрок
class Player : Entity
{
    public Weapon Weapon { get; set; }
    public Armor Armor { get; set; }
    public bool IsFrozen { get; set; }

    public Player() : base(100, 10, 3) // базовые значения
    {
        Weapon = new Weapon("Кулаки", 0);
        Armor = new Armor("Тряпье", 0);
        IsFrozen = false;
    }

    public override void AttackTarget(Entity target, Random rand)
    {
        Enemy enemy = target as Enemy;

        int damage = Math.Max(1, Attack - target.Defense);

        if (enemy.DamageReduction == true)
        {
            damage -= 2;
        }
        else
        {
            damage = Math.Max(1, Attack - target.Defense);
        }
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
    public bool DamageReduction { get; }

    public Enemy(string name, int hp, int attack, int defense, double critChance = 0, double freezeChance = 0, bool ignoresDefense = false, bool damageReduction = false)
        : base(hp, attack, defense)
    {
        Name = name;
        CritChance = critChance;
        FreezeChance = freezeChance;
        IgnoresDefense = ignoresDefense;
        DamageReduction = damageReduction;
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

        if (DamageReduction)
        {
            Console.WriteLine($"{Name} поглощает 2 единицы урона!");
        }


        player.HP -= actualDamage;
        Console.WriteLine($"{Name} наносит {actualDamage} урона!\n");

        if (rand.NextDouble() < FreezeChance)
        {
            player.IsFrozen = true;
            Console.WriteLine($"{Name} замораживает вас! Вы пропускаете следующий ход.");
        }
    }
}
