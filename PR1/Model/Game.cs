using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class Game
{
    private Player player;
    private EnemyFactory factory;
    private Random RandomProvider;
    private int turn;

    public Game()
    {
        player = new Player();
        factory = new EnemyFactory();
        RandomProvider = new Random();
        turn = 0;
    }

    // метод для генерации лута
    private object GenerateLoot()
    {
        int type = RandomProvider.Next(3);
        if (type == 0) return new Potion();
        else if (type == 1) return new Weapon($"Меч {RandomProvider.Next(5, 21)}", RandomProvider.Next(5, 21));
        else return new Armor($"Броня {RandomProvider.Next(2, 11)}", RandomProvider.Next(2, 11));
    }

    // метод для обработки сундука
    private void HandleChest()
    {
        Console.WriteLine("Вы нашли сундук!");
        object loot = GenerateLoot();
        if (loot is Potion potion)
        {
            potion.Use(player);
        }
        else if (loot is IItem item)
        {
            Console.WriteLine($"Вы нашли: {item}");
            if (item is Weapon)
            {
                Console.WriteLine($"Текущее оружие: {player.Weapon}");
            }
            else if (item is Armor)
            {
                Console.WriteLine($"Текущие доспехи: {player.Armor}");
            }
            Console.Write("Взять? (Y/N): ");
            if (Console.ReadLine().ToUpper() == "Y")
            {
                item.Equip(player);
                Console.WriteLine("Предмет экипирован!");
            }
            else
            {
                Console.WriteLine("Предмет выброшен.");
            }
        }
    }

    // метод для боя
    private void Battle(Enemy enemy)
    {
        Console.WriteLine($"Вы столкнулись с {enemy.Name}!");
        Console.WriteLine($"HP: {enemy.MaxHP} Атака: {enemy.Attack} Защита: {enemy.Defense}\n");

        while (player.HP > 0 && enemy.HP > 0)
        {
            player.DisplayHP("Ваше HP");
            enemy.DisplayHP("HP врага");

            if (!player.IsFrozen)
            {
                Console.Write("Ваш ход: (A)ttack или (D)efense? ");
                string choice = Console.ReadLine().ToUpper();
                if (choice == "A")
                {
                    player.AttackTarget(enemy, RandomProvider);
                }
                else if (choice == "D")
                {
                    player.Defend(RandomProvider); // защита, но урон будет учтен в атаке врага
                }
                else
                {
                    Console.WriteLine("Неверный выбор, считаем атакой.");
                    player.AttackTarget(enemy, RandomProvider);
                }
            }
            else
            {
                Console.WriteLine("Вы заморожены и пропускаете ход!");
                player.IsFrozen = false;
            }

            if (enemy.HP > 0)
            {
                enemy.AttackTarget(player, RandomProvider);
            }

            if (player.HP <= 0)
            {
                Console.WriteLine("Вы погибли! Игра окончена.");
                return;
            }
        }

        if (enemy.HP <= 0)
        {
            Console.WriteLine($"Вы победили {enemy.Name}!");
        }
    }

    // основной игровой цикл
    public void Run()
    {
        Console.WriteLine("Добро пожаловать в текстовую игру!");
        Console.WriteLine("Каждый ход: сундук или враг. Каждые 10 ходов - босс.");

        while (player.HP > 0)
        {
            turn++;
            Console.Clear();
            Console.WriteLine("Добро пожаловать в текстовую игру!");
            Console.WriteLine("Каждый ход: сундук или враг. Каждые 10 ходов - босс.");
            Console.WriteLine($"\n--- Ход {turn} ---");

            bool isBoss = (turn % 10 == 0);
            bool isChest = RandomProvider.Next(2) == 0;

            if (isBoss)
            {
                Console.WriteLine("Вам встречается босс!");
                Enemy boss = factory.GenerateBoss();
                Battle(boss);
            }
            else if (isChest)
            {
                HandleChest();
            }
            else
            {
                Enemy enemy = factory.GenerateEnemy();
                Battle(enemy);
            }

            if (player.HP <= 0) break;
        }
    }
}

