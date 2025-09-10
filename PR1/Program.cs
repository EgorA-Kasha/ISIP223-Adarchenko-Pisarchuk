using System.Diagnostics.Contracts;

Console.Write("Введите кол-во операций: ");
var n = Convert.ToInt32(Console.ReadLine());

if (n < 2 || n > 40) {
    Console.WriteLine("Таки надо от 2 до 40 операций");
    
}

var data = new List<Tuple<string, double>>();

for (int i = 0; i < n; i++) {
    var name_amt = Console.ReadLine().Split();
    var amt = Convert.ToDouble(name_amt[1]);
    data.Add(new Tuple<string, double>(name_amt[0], amt));
}

int v = -1;

var cur_list = new List<Tuple<string, double>> {
    new Tuple<string, double>("ДоллАры", 25),
    new Tuple<string, double>("Евры"   , 30)
};

while (v != 0)
{
    Console.WriteLine("""
1. Вывод данных    
2. Статистика
3. Сортировка по цене
4. Конвертация валюты
5. Поиск по названию
0. Выход
""");
    v = Convert.ToInt32(Console.ReadLine());
    switch (v)
    {
        case 1:
            foreach (var item in data)
                Console.WriteLine($"{item.Item1} - {item.Item2} руб.");
            break;
        case 2:
            var list = new List<double>();
            foreach (var item in data)
                list.Add(item.Item2);
            Console.WriteLine($"Максимальное: {list.Max()}, минимальное: {list.Min()}, сумма: {list.Sum()}, среднее: {list.Average()}");
            break;
        case 3:
            var copy = new List<Tuple<string, double>>();
            var z = data.Count;
            for (int i = 0; i < z; i++)
            {
                int min_i = 0;
                for (int j = 0; j < data.Count; j++)
                    if (data[j].Item2 < data[min_i].Item2)
                        min_i = j;
                copy.Add(data[min_i]);
                data.RemoveAt(min_i);
            }
            data = copy;
            break;
        case 4:
            Console.WriteLine("Введите номер валюты из списка");
            Console.WriteLine("0\tВвести своё");
            for (int i = 0; i < cur_list.Count; i++)
                Console.WriteLine($"{i + 1}\t{cur_list[i].Item1}\t{cur_list[i].Item2}");
            var a = Convert.ToInt32(Console.ReadLine());
            double k;
            if (a != 0)
                k = cur_list[a - 1].Item2;
            else
            {
                Console.Write("Введите курс (рублей за у.е.): ");
                k = Convert.ToDouble(Console.ReadLine());
            }
            foreach (var item in data)
                Console.WriteLine($"{item.Item1} - {item.Item2 / k}");
            break;
        case 5:
            Console.Write("Введите название товара: ");
            var b = Console.ReadLine();
            foreach (var item in data)
                if (item.Item1.Contains(b))
                    Console.WriteLine($"{item.Item1} - {item.Item2} руб.");
            break;
    }
}

