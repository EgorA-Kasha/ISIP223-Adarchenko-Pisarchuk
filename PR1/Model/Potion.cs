using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    internal class Potion
{
    public string Name => "Зелье исцеления";

    public void Use(Player player)
    {
        player.HP = player.MaxHP;
        Console.WriteLine("Зелье исцеления! HP восстановлено!");
    }

    public override string ToString() => Name;
}
