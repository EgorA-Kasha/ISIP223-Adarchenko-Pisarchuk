Console.Write("Введите кол-во операций: ");
var n = Convert.ToInt64(Console.ReadLine());

if (n < 2 || n > 40) {
    Console.WriteLine("Таки надо от 2 до 40 операций");
    
}

for (int i = 0; i < n; i++) {
    var name_amt = Console.ReadLine().Split();
    var amt = Convert.ToInt64(name_amt[1]);
}

Console.Write("1. Вывод данных\n""2. Статистика (среднее, максимальное, минимальное, сумма)3. Сортировка по цене (пузырьковая сортировка)4. Конвертация валюты (пользователь вводит курс или выбирает из списка)5. Поиск по названию 0. Выход")
