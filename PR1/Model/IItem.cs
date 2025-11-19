using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal interface IItem
{
    string Name { get; }
    void Equip(Player player);
}

internal class Weapon : IItem
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

internal class Armor : IItem
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