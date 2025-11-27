using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedRPG
{
    // класс игры
    internal class Game
    {
        private Player player;
        private EnemyFactory factory;
        private int turn;

        public Game()
        {
            player = new Player("Игрок", 100, 10, 0);
            factory = new EnemyFactory();
            turn = 0;
        }

        // генерации лута
        private IItem GenerateLoot()
        {
            int type = RandomHelper.Next(3);
            if (type == 0) return new Potion();
            else if (type == 1) return new Weapon($"Меч +{RandomHelper.Next(5, 21)}", RandomHelper.Next(5, 21));
            else return new Armor($"Броня +{RandomHelper.Next(2, 11)}", RandomHelper.Next(2, 11));
        }

        // сундук
        private void HandleChest()
        {
            Console.WriteLine("Вы нашли сундук!");
            IItem loot = GenerateLoot();
            if (loot is Potion potion)
            {
                potion.Use(player);
                Console.WriteLine("Нажмите Enter для продолжения...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Вы нашли: {loot}");
                if (loot is Weapon)
                {
                    Console.WriteLine($"Текущее оружие: {player.Weapon?.Name ?? "Нет"}");
                }
                else if (loot is Armor)
                {
                    Console.WriteLine($"Текущие доспехи: {player.Armor?.Name ?? "Нет"}");
                }
                Console.Write("Взять? (Y/N): ");
                string input = Console.ReadLine()?.ToUpper() ?? "";
                if (input == "Y")
                {
                    loot.Equip(player);
                    Console.WriteLine("Предмет экипирован!");
                }
                else
                {
                    Console.WriteLine("Предмет выброшен.");
                }
            }
            Console.WriteLine();
        }

        // бой
        private void Battle(AbstractEnemy enemy)
        {
            Console.WriteLine($"Вы столкнулись с {enemy.Name}!");
            player.DisplayStatus();
            enemy.DisplayStatus();

            while (player.IsAlive && enemy.IsAlive)
            {
                int playerDefend = 0;
                bool playerTurnTaken = false;

                // обработка хода игрока
                if (player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    player.Unfreeze();
                    playerTurnTaken = true;
                }
                else
                {
                    while (!playerTurnTaken)
                    {
                        Console.Write("Ваш ход: (A)ttack или (D)efense? ");
                        string choice = Console.ReadLine()?.ToLower().Trim() ?? "";
                        if (choice.StartsWith("a") || choice.Contains("attack"))
                        {
                            int net = enemy.TakeDamage(player.GetAttack());
                            Console.WriteLine($"Вы наносите {net} урона!");
                            playerTurnTaken = true;
                        }
                        else if (choice.StartsWith("d") || choice.Contains("defense"))
                        {
                            playerDefend = 1;
                            Console.WriteLine("Вы защищаетесь, снижая урон противника.");
                            playerTurnTaken = true;
                        }
                        else
                        {
                            Console.WriteLine("Неверная команда, повторяем ход.");
                        }
                    }
                }

                // проверка после хода игрока
                if (!enemy.IsAlive)
                {
                    player.DisplayStatus();
                    enemy.DisplayStatus();
                    Console.WriteLine("Вы победили!");
                    return;
                }

                // ход врага
                enemy.AttackWithDefend(player, playerDefend);
                Console.WriteLine();

                // проверка после хода врага
                if (!player.IsAlive)
                {
                    player.DisplayStatus();
                    enemy.DisplayStatus();
                    Console.WriteLine("Игра окончена. Вы проиграли.");
                    return;
                }

                player.DisplayStatus();
                enemy.DisplayStatus();
            }
        }

        // основной игровой цикл
        public void Run()
        {
            while (player.IsAlive)
            {
                turn++;
                Console.Clear();
                Console.WriteLine("Добро пожаловать в текстовую игру!");
                Console.WriteLine("Каждый ход: сундук или враг. Каждые 10 ходов - босс.");
                Console.WriteLine($"\n--- Ход {turn} ---");

                bool isBoss = (turn % 10 == 0);
                bool isChest = !isBoss && RandomHelper.Next(2) == 0;

                if (isBoss)
                {
                    Console.WriteLine("Вам встречается босс!");
                    AbstractEnemy boss = factory.GenerateBoss();
                    Battle(boss);
                }
                else if (isChest)
                {
                    HandleChest();
                }
                else
                {
                    AbstractEnemy enemy = factory.GenerateEnemy();
                    Battle(enemy);
                }

                if (!player.IsAlive) break;
            }
        }
    }
}
