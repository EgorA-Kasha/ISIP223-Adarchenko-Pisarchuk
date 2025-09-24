using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    enum Genre
    {
        Fiction = 1,
        NonFiction,
        ScienceFiction,
        Fantasy,
        Biography
    }

    class Book
    {
        private static int _idCounter = 1;

        public int Id { get; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        public Book(string title, string author, Genre genre, int year, decimal price)
        {
            Id = _idCounter++;
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            Price = price;
        }

        public override string ToString()
        {
            return $"ID: {Id}\n" +
                   $"Название: {Title}\n" +
                   $"Автор: {Author}\n" +
                   $"Жанр: {Genre}\n" +
                   $"Год издания: {Year}\n" +
                   $"Цена: {Price:C}\n";
        }
    }

    class Program
    {
        static List<Book> books = new List<Book>();
        static List<Book> cart = new List<Book>();

        static void Main(string[] args)
        {
            // Добавляем 5 тестовых книг
            books.Add(new Book("Мастер и Маргарита", "Михаил Булгаков",    Genre.Fiction, 1967, 500));
            books.Add(new Book("Краткая история времени", "Стивен Хокинг", Genre.NonFiction, 1988, 700));
            books.Add(new Book("Властелин колец", "Дж. Р. Р. Толкин",      Genre.Fantasy, 1954, 1200));
            books.Add(new Book("1984", "Джордж Оруэлл",                    Genre.ScienceFiction, 1949, 450));
            books.Add(new Book("Стив Джобс", "Уолтер Айзексон",            Genre.Biography, 2011, 800));

            Console.WriteLine("Добро пожаловать в библиотеку!");
            while (true)
            {
                Console.WriteLine("\nДоступные команды:");
                Console.WriteLine("1 - Добавить книгу");
                Console.WriteLine("2 - Удалить книгу по ID");
                Console.WriteLine("3 - Найти книги");
                Console.WriteLine("4 - Отсортировать книги");
                Console.WriteLine("5 - Показать самую дорогую и самую дешевую книгу");
                Console.WriteLine("6 - Группировать книги по авторам");
                Console.WriteLine("7 - Пакетный импорт");
                Console.WriteLine("8 - Корзина");
                Console.WriteLine("0 - Выход");

                Console.Write("Введите номер команды: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddBook();
                        break;
                    case "2":
                        DeleteBook();
                        break;
                    case "3":
                        FindBooks();
                        break;
                    case "4":
                        SortBooks();
                        break;
                    case "5":
                        ShowPriceExtremes();
                        break;
                    case "6":
                        GroupByAuthors();
                        break;
                    case "7":
                        BatchImport();
                        break;
                    case "8":
                        AddToCart();
                        break;
                    case "0":
                        Console.WriteLine("Спасибо за использование программы. До свидания!");
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда. Попробуйте снова.");
                        break;
                }
            }
        }

        static void AddBook()
        {
            Console.WriteLine("\nДобавление новой книги:");

            string title = ReadNonEmptyString("Введите название книги: ");
            string author = ReadNonEmptyString("Введите автора книги: ");

            Genre genre = ReadGenre();

            int year = ReadInt("Введите год издания (например, 1999): ", 1, DateTime.Now.Year);

            decimal price = ReadDecimal("Введите цену книги (положительное число): ", 0.01m, decimal.MaxValue);

            Book newBook = new Book(title, author, genre, year, price);
            books.Add(newBook);
            Console.WriteLine("\nКнига успешно добавлена:");
            Console.WriteLine(newBook);
        }

        static void DeleteBook()
        {
            Console.Write("\nВведите ID книги для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Book bookToRemove = books.FirstOrDefault(b => b.Id == id);
                if (bookToRemove != null)
                {
                    books.Remove(bookToRemove);
                    Console.WriteLine($"Книга с ID {id} удалена.");
                }
                else
                {
                    Console.WriteLine($"Книга с ID {id} не найдена.");
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод ID.");
            }
        }

        static void FindBooks()
        {
            Console.WriteLine("\nПоиск книг. Выберите параметр поиска:");
            Console.WriteLine("1 - По названию");
            Console.WriteLine("2 - По автору");
            Console.WriteLine("3 - По жанру");

            Console.Write("Введите номер параметра: ");
            string choice = Console.ReadLine();

            IEnumerable<Book> foundBooks = Enumerable.Empty<Book>();

            switch (choice)
            {
                case "1":
                    string title = ReadNonEmptyString("Введите название для поиска: ").ToLower();
                    foundBooks = books.Where(b => b.Title.ToLower().Contains(title));
                    break;
                case "2":
                    string author = ReadNonEmptyString("Введите автора для поиска: ").ToLower();
                    foundBooks = books.Where(b => b.Author.ToLower().Contains(author));
                    break;
                case "3":
                    Genre genre = ReadGenre();
                    foundBooks = books.Where(b => b.Genre == genre);
                    break;
                default:
                    Console.WriteLine("Некорректный выбор параметра поиска.");
                    return;
            }

            if (foundBooks.Any())
            {
                Console.WriteLine($"\nНайдено книг: {foundBooks.Count()}");
                foreach (var book in foundBooks)
                {
                    Console.WriteLine(book);
                }
            }
            else
            {
                Console.WriteLine("Книги по заданным параметрам не найдены.");
            }
        }

        static void SortBooks()
        {
            Console.WriteLine("\nСортировка книг. Выберите параметр сортировки:");
            Console.WriteLine("1 - По названию (по алфавиту)");
            Console.WriteLine("2 - По году издания (по возрастанию)");

            Console.Write("Введите номер параметра: ");
            string choice = Console.ReadLine();

            IEnumerable<Book> sortedBooks = Enumerable.Empty<Book>();

            switch (choice)
            {
                case "1":
                    sortedBooks = books.OrderBy(b => b.Title);
                    break;
                case "2":
                    sortedBooks = books.OrderBy(b => b.Year);
                    break;
                default:
                    Console.WriteLine("Некорректный выбор параметра сортировки.");
                    return;
            }

            Console.WriteLine("\nОтсортированные книги:");
            foreach (var book in sortedBooks)
            {
                Console.WriteLine(book);
            }
        }

        static void ShowPriceExtremes()
        {
            if (!books.Any())
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }

            var mostExpensive = books.OrderByDescending(b => b.Price).First();
            var cheapest = books.OrderBy(b => b.Price).First();

            Console.WriteLine("\nСамая дорогая книга:");
            Console.WriteLine(mostExpensive);

            Console.WriteLine("Самая дешевая книга:");
            Console.WriteLine(cheapest);
        }
        static void GroupByAuthors()
        {
            if (!books.Any())
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }

            var groups = books.GroupBy(b => b.Author)
                              .Select(g => new { Author = g.Key, Count = g.Count() })
                              .OrderByDescending(g => g.Count);

            Console.WriteLine("\nКоличество книг по авторам:");
            foreach (var group in groups)
            {
                Console.WriteLine($"Автор: {group.Author}, Количество книг: {group.Count}");
            }
        }

        static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(input))
                    return input;
                Console.WriteLine("Ввод не может быть пустым. Попробуйте снова.");
            }
        }

        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int value))
                {
                    if (value >= min && value <= max)
                        return value;
                    else
                        Console.WriteLine($"Значение должно быть в диапазоне от {min} до {max}.");
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Введите целое число.");
                }
            }
        }

        static decimal ReadDecimal(string prompt, decimal min, decimal max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (decimal.TryParse(input, out decimal value))
                {
                    if (value >= min && value <= max)
                        return value;
                    else
                        Console.WriteLine($"Значение должно быть в диапазоне от {min} до {max}.");
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Введите число.");
                }
            }
        }

        static Genre ReadGenre()
        {
            Console.WriteLine("Выберите жанр из списка:");

            foreach (var val in Enum.GetValues(typeof(Genre)))
            {
                Console.WriteLine($"{(int)val} - {val}");
            }

            while (true)
            {
                Console.Write("Введите номер жанра: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int genreNum) &&
                    Enum.IsDefined(typeof(Genre), genreNum))
                {
                    return (Genre)genreNum;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Попробуйте снова.");
                }
            }
        }
        static void BatchImport()
        {
            Console.WriteLine("\nВведите книги для пакетного импорта в формате:");
            Console.WriteLine("Название;Автор;Жанр;Год;Цена");
            Console.WriteLine("Введите пустую строку для завершения ввода.");

            int addedCount = 0;
            while (true)
            {
                Console.Write("Введите книгу: ");
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    break;

                string[] parts = line.Split(';');
                if (parts.Length != 5)
                {
                    Console.WriteLine("Ошибка: неверное количество параметров. Ожидается 5 через ';'");
                    break;
                }

                string title = parts[0].Trim();
                string author = parts[1].Trim();
                string genreStr = parts[2].Trim();
                string yearStr = parts[3].Trim();
                string priceStr = parts[4].Trim();

                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author))
                {
                    Console.WriteLine("Ошибка: название и автор не могут быть пустыми.");
                    break;
                }

                if (!Enum.TryParse<Genre>(genreStr, true, out Genre genre))
                {
                    Console.WriteLine($"Ошибка: жанр '{genreStr}' не распознан. Доступные жанры:");
                    foreach (var g in Enum.GetNames(typeof(Genre)))
                        Console.WriteLine($"- {g}");
                    break;
                }

                if (!int.TryParse(yearStr, out int year) || year < 1 || year > DateTime.Now.Year)
                {
                    Console.WriteLine("Ошибка: год должен быть числом от 1 до текущего года.");
                    break;
                }

                if (!decimal.TryParse(priceStr, out decimal price) || price <= 0)
                {
                    Console.WriteLine("Ошибка: цена должна быть положительным числом.");
                    break;
                }

                books.Add(new Book(title, author, genre, year, price));
                addedCount++;
            }
            Console.WriteLine($"Пакетный импорт завершён. Добавлено книг: {addedCount}");
        }
        static void AddToCart()
        {
            Console.Write("\nВведите ID книги для добавления в корзину: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Book book = books.FirstOrDefault(b => b.Id == id);
                if (book != null)
                {
                    cart.Add(book);
                    Console.WriteLine($"Книга '{book.Title}' добавлена в корзину.");
                    ShowCart(); // вывод корзины сразу после добавления
                }
                else
                {
                    Console.WriteLine($"Книга с ID {id} не найдена.");
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод ID.");
            }
        }

        static void ShowCart()
        {
            Console.WriteLine("\nСодержимое корзины:");
            if (!cart.Any())
            {
                Console.WriteLine("Корзина пуста.");
                return;
            }

            var grouped = cart.GroupBy(b => b.Id).Select(g => new
                                                              {
                                                                  Book = g.First(),
                                                                  Count = g.Count()
                                                              });

            decimal total = 0;

            foreach (var item in grouped)
            {
                decimal sumPrice = item.Book.Price * item.Count;
                total += sumPrice;
                Console.WriteLine($"ID: {item.Book.Id} | Название: {item.Book.Title} | Автор: {item.Book.Author} | Кол-во: {item.Count} | Цена за шт: {item.Book.Price:C} | Сумма: {sumPrice:C}");
            }

            Console.WriteLine($"Общая стоимость корзины: {total:C}");
        }
    }
}