using System;

class Product
{
    public string Code { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public bool InStock { get; set; }
    public string Category { get; set; }

    public Product(string name, double price, int quantity, string category)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        Category = category;
        InStock = quantity > 0;
    }

    public void UpdateStock()
    {
        InStock = Quantity > 0;
    }



    public override string ToString()
    {
        return $"Код: {Code}\nНазвание: {Name}\nЦена: {Price:C}\nКоличество: {Quantity}\nНа складе: {(InStock ? "Да" : "Нет")}\nКатегория: {Category}\n";
    }
}

class Program
{
    static List<Product> products = new List<Product>();
    static List<string> categories = new List<string> { "Электроника", "Одежда", "Продукты" };

    static Random random = new Random();

    static Stack<SaleRecord> salesHistory = new Stack<SaleRecord>();

    class SaleRecord
    {
        public string ProductCode { get; }
        public string ProductName { get; }
        public int QuantitySold { get; }
        public double TotalPrice { get; }

        public SaleRecord(string code, string name, int quantity, double totalPrice)
        {
            ProductCode = code;
            ProductName = name;
            QuantitySold = quantity;
            TotalPrice = totalPrice;
        }
    }

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Меню управления товарами:");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");
            Console.WriteLine("3. Заказать поставку товара");
            Console.WriteLine("4. Продать товар");
            Console.WriteLine("5. Поиск товаров");
            Console.WriteLine("6. Отменить последнюю продажу");
            Console.WriteLine("7. Получить отчёт о продажах");
            Console.WriteLine("8. Выход");
            Console.Write("Выберите опцию: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddProduct();
                    break;
                case "2":
                    RemoveProduct();
                    break;
                case "3":
                    OrderSupply();
                    break;
                case "4":
                    SellProduct();
                    break;
                case "5":
                    SearchProducts();
                    break;
                case "6":
                    UndoLastSale();
                    break;
                case "7":
                    ShowSalesReport();
                    break;
                case "8":
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Нажмите Enter для продолжения.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void AddProduct()
    {
        Console.Clear();
        Console.WriteLine("Добавление товара:");

        // запрос кода с коррекцией (добавляем "1" в начало если не начинается с "1")
        string code;
        while (true)
        {
            Console.Write("Код товара (введите цифры): ");
            string inputCode = Console.ReadLine().Trim();

            // убираем не цифры
            inputCode = new string(inputCode.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(inputCode))
            {
                Console.WriteLine("Код не может быть пустым. Введите цифры.");
                continue;
            }

            // если не начинается с "1" добавляем "1" в начало
            if (!inputCode.StartsWith("1"))
            {
                inputCode = "1" + inputCode;
            }

            // если длина > 6 обрезаем до 6
            if (inputCode.Length > 6)
            {
                Console.WriteLine("Код слишком длинный. Он будет обрезан до 6 цифр.");
                inputCode = inputCode.Substring(0, 6);
            }

            // если длина < 6 дополняем случайными цифрами
            while (inputCode.Length < 6)
            {
                inputCode += random.Next(0, 10).ToString();
            }

            code = inputCode;

            // проверяем уникальность
            if (products.Any(p => p.Code == code))
            {
                Console.WriteLine("Этот код уже используется. Введите другой код.");
                continue;
            }
            break;
        }

        Console.Write("Название: ");
        string name = Console.ReadLine();

        Console.Write("Цена: ");
        double price;
        while (!double.TryParse(Console.ReadLine(), out price) || price < 0)
        {
            Console.Write("Введите корректную цену: ");
        }

        Console.Write("Количество: ");
        int quantity;
        while (!int.TryParse(Console.ReadLine(), out quantity) || quantity < 0)
        {
            Console.Write("Введите корректное количество: ");
        }

        string category;
        while (true)
        {
            Console.WriteLine("Категории:");
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i]}");
            }
            Console.Write("Выберите категорию (номер): ");
            string catInput = Console.ReadLine();
            if (int.TryParse(catInput, out int catIndex) && catIndex >= 1 && catIndex <= categories.Count)
            {
                category = categories[catIndex - 1];
                break;
            }
            else
            {
                Console.WriteLine("Неверный выбор. Выберите номер из списка.");
            }
        }

        Product product = new Product(name, price, quantity, category);
        product.Code = code;
        products.Add(product);

        Console.WriteLine($"Товар добавлен! Нажмите Enter для продолжения.");
        Console.ReadLine();
    }

    static void RemoveProduct()
    {
        Console.Clear();
        Console.Write("Введите код товара для удаления: ");
        string code = Console.ReadLine();
        Product product = products.FirstOrDefault(p => p.Code == code);
        if (product != null)
        {
            products.Remove(product);
            Console.WriteLine("Товар удалён! Нажмите Enter для продолжения.");
        }
        else
        {
            Console.WriteLine("Товар не найден. Нажмите Enter для продолжения.");
        }
        Console.ReadLine();
    }

    static void OrderSupply()
    {
        Console.Clear();
        Console.Write("Введите код товара для поставки: ");
        string code = Console.ReadLine();
        Product product = products.FirstOrDefault(p => p.Code == code);
        if (product != null)
        {
            Console.Write("Количество для поставки: ");
            int supply;
            while (!int.TryParse(Console.ReadLine(), out supply) || supply <= 0)
            {
                Console.Write("Введите корректное количество: ");
            }
            product.Quantity += supply;
            product.UpdateStock();
            Console.WriteLine("Поставка выполнена! Нажмите Enter для продолжения.");
        }
        else
        {
            Console.WriteLine("Товар не найден. Нажмите Enter для продолжения.");
        }
        Console.ReadLine();
    }

    static void SellProduct()
    {
        Console.Clear();
        Console.WriteLine("Продажа товара:");

        Console.Write("Введите код товара для продажи: ");
        string code = Console.ReadLine();

        Product product = products.FirstOrDefault(p => p.Code == code);

        if (product == null)
        {
            Console.WriteLine("Товар с таким кодом не найден.");
        }
        else if (product.Quantity == 0)
        {
            Console.WriteLine("Ошибка: товар отсутствует на складе и не может быть продан.");
        }
        else
        {
            Console.Write("Введите количество для продажи: ");
            if (int.TryParse(Console.ReadLine(), out int sellQuantity))
            {
                if (sellQuantity <= 0)
                {
                    Console.WriteLine("Количество для продажи должно быть положительным числом.");
                }
                else if (sellQuantity > product.Quantity)
                {
                    Console.WriteLine($"Ошибка: на складе недостаточно товара. Доступно: {product.Quantity}.");
                }
                else
                {
                    double totalPrice = sellQuantity * product.Price;

                    product.Quantity -= sellQuantity;
                    product.UpdateStock();

                    // записываем продажу в историю
                    salesHistory.Push(new SaleRecord(product.Code, product.Name, sellQuantity, totalPrice));

                    Console.WriteLine($"Продано {sellQuantity} единиц товара \"{product.Name}\" на сумму {totalPrice:C}.");
                }
            }
            else
            {
                Console.WriteLine("Введено некорректное количество.");
            }
        }

        Console.WriteLine("Нажмите Enter для продолжения.");
        Console.ReadLine();
    }

    static void SearchProducts()
    {
        Console.Clear();
        Console.WriteLine("Поиск товаров:");
        Console.WriteLine("1. По коду");
        Console.WriteLine("2. По названию");
        Console.WriteLine("3. По категории");
        Console.Write("Выберите тип поиска: ");
        string searchType = Console.ReadLine();

        List<Product> results = new List<Product>();

        switch (searchType)
        {
            case "1":
                Console.Write("Введите код: ");
                string code = Console.ReadLine();
                results = products.Where(p => p.Code == code).ToList();
                break;
            case "2":
                Console.Write("Введите название: ");
                string name = Console.ReadLine();
                results = products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case "3":
                Console.WriteLine("Категории:");
                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {categories[i]}");
                }
                Console.Write("Выберите категорию (номер): ");
                int catIndex;
                while (!int.TryParse(Console.ReadLine(), out catIndex) || catIndex < 1 || catIndex > categories.Count)
                {
                    Console.Write("Введите корректный номер категории: ");
                }
                string category = categories[catIndex - 1];
                results = products.Where(p => p.Category == category).ToList();
                break;
            default:
                Console.WriteLine("Неверный выбор.");
                Console.ReadLine();
                return;
        }

        if (results.Count > 0)
        {
            foreach (var product in results)
            {
                Console.WriteLine(product);
            }
        }
        else
        {
            Console.WriteLine("Товары не найдены.");
        }

        Console.WriteLine("Нажмите Enter для продолжения.");
        Console.ReadLine();
    }

    static void UndoLastSale()
    {
        Console.Clear();
        if (salesHistory.Count == 0)
        {
            Console.WriteLine("Нет продаж для отмены.");
        }
        else
        {
            SaleRecord lastSale = salesHistory.Pop();

            Product product = products.FirstOrDefault(p => p.Code == lastSale.ProductCode);
            if (product != null)
            {
                product.Quantity += lastSale.QuantitySold;
                product.UpdateStock();

                Console.WriteLine($"Отмена последней продажи: товар \"{product.Name}\", количество {lastSale.QuantitySold} возвращено на склад.");
            }
            else
            {
                Console.WriteLine("Ошибка: товар из истории продаж не найден в базе.");
            }
        }

        Console.WriteLine("Нажмите Enter для продолжения.");
        Console.ReadLine();
    }

    static void ShowSalesReport()
    {
        Console.Clear();
        if (salesHistory.Count == 0)
        {
            Console.WriteLine("Продаж пока не было.");
        }
        else
        {
            // группируем продажи по коду и имени товара
            var groupedSales = salesHistory
                .GroupBy(s => new { s.ProductCode, s.ProductName })
                .Select(g => new
                {
                    ProductCode = g.Key.ProductCode,
                    ProductName = g.Key.ProductName,
                    TotalQuantity = g.Sum(s => s.QuantitySold),
                    TotalSum = g.Sum(s => s.TotalPrice)
                })
                .ToList();

            double grandTotal = groupedSales.Sum(s => s.TotalSum);

            Console.WriteLine("Отчёт о продажах:");
            Console.WriteLine("Код\tНазвание\tКоличество\tСумма");

            foreach (var item in groupedSales)
            {
                Console.WriteLine($"{item.ProductCode}\t{item.ProductName}\t{item.TotalQuantity}\t\t{item.TotalSum:C}");
            }

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"Общая сумма продаж: {grandTotal:C}");
        }

        Console.WriteLine("Нажмите Enter для продолжения.");
        Console.ReadLine();
    }
}