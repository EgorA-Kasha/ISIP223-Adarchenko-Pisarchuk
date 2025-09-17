using System.Text.RegularExpressions;

class TextStatistics
{
    public string originalText { get; set; }
    public int wordCount { get; set; }
    public string shortestWord { get; set; }
    public int sentenceCount { get; set; }
    public int glasCount { get; set; }
    public int soglCount { get; set; }
    public string longestWord { get; set; }
    public Dictionary<char, int> letterFrequency { get; set; }

    public TextStatistics(string text)
    {
        originalText = text;
        AnalyzeText();
    }

    private void AnalyzeText()
    {
        // Подсчёт слов: разделяем по пробелам и знакам препинания
        var words = Regex.Split(originalText, @"\W+").Where(w => !string.IsNullOrEmpty(w)).ToArray();
        wordCount = words.Length;

        // Самое короткое слово
        shortestWord = words.OrderBy(w => w.Length).FirstOrDefault();

        // Самое длинное слово
        longestWord = words.OrderByDescending(w => w.Length).FirstOrDefault();

        // Подсчёт предложений: по точкам, восклицательным и вопросительным знакам
        sentenceCount = Regex.Matches(originalText, @"[.!?]").Count;

        // Подсчёт гласных и согласных (для английского и русского)
        glasCount = originalText.Count(c => "aeiouyаеёиоуыэюя".Contains(char.ToLower(c)));
        soglCount = originalText.Count(c => char.IsLetter(c) && !"aeiouyаеёиоуыэюя".Contains(char.ToLower(c)));

        // Частота букв
        letterFrequency = new Dictionary<char, int>();
        foreach (char c in originalText.Where(char.IsLetter))
        {
            char lower = char.ToLower(c);
            if (letterFrequency.ContainsKey(lower))
                letterFrequency[lower]++;
            else
                letterFrequency[lower] = 1;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<TextStatistics> statisticsList = new List<TextStatistics>();

        while (true)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Ввести новый текст и проанализировать");
            Console.WriteLine("2. Вывести статистику по прошлым текстам");
            Console.WriteLine("3. Выйти\n");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine("\nВведите текст (минимум 100 символов):");
                string input = Console.ReadLine();

                if (input.Length < 100)
                {
                    Console.WriteLine("Текст слишком короткий. Попробуйте снова.");
                    continue;
                }

                TextStatistics stats = new TextStatistics(input);
                statisticsList.Add(stats);

                Console.WriteLine("\nСтатистика для введённого текста:");
                Console.WriteLine($"Количество слов: {stats.wordCount}");
                Console.WriteLine($"Самое короткое слово: {stats.shortestWord}");
                Console.WriteLine($"Количество предложений: {stats.sentenceCount}");
                Console.WriteLine($"Гласных букв: {stats.glasCount}");
                Console.WriteLine($"Согласных букв: {stats.soglCount}");
                Console.WriteLine($"Самое длинное слово: {stats.longestWord}\n");
                Console.WriteLine("Частота букв:");
                foreach (var pair in stats.letterFrequency.OrderBy(p => p.Key))
                {
                    Console.WriteLine($"  {pair.Key}: {pair.Value}");
                }
            }
            else if (choice == "2")
            {
                if (statisticsList.Count == 0)
                {
                    Console.WriteLine("Нет сохранённых статистик.");
                }
                else
                {
                    Console.WriteLine("\nСтатистика по прошлым текстам:");
                    for (int i = 0; i < statisticsList.Count; i++)
                    {
                        Console.WriteLine($"\nТекст {i + 1}: {statisticsList[i].originalText.Substring(0, Math.Min(50, statisticsList[i].originalText.Length))}...");
                        Console.WriteLine($"Количество слов: {statisticsList[i].wordCount}");
                        Console.WriteLine($"Самое короткое слово: {statisticsList[i].shortestWord}");
                        Console.WriteLine($"Количество предложений: {statisticsList[i].sentenceCount}");
                        Console.WriteLine($"Гласных: {statisticsList[i].glasCount}, Согласных: {statisticsList[i].soglCount}");
                        Console.WriteLine($"Самое длинное слово: {statisticsList[i].longestWord}");
                    }
                }
            }
            else if (choice == "3")
            {
                break;
            }
            else
            {
                Console.WriteLine("Неверный выбор. Попробуйте снова.");
            }
        }
    }
}

