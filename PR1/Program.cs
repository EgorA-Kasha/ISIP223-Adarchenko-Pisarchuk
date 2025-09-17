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

    private static readonly List<string> excludedWords = new List<string>
        {
            "и", "а", "но", "или", "что", "как", "когда", "где", "потому", "чтобы", 
            "если", "то", "так", "уже", "ещё", "да", "нет", "в", "на", "с", "по", "из", 
            "к", "от", "у", "о", "за", "под", "над", "перед", "после", "между"
        };

    public TextStatistics(string text)
    {
        originalText = text;
        AnalyzeText();
    }

    private void AnalyzeText()
    {
        // подсчёт слов: разделяем по пробелам и знакам препинания, исключаем союзы и числа
        var words = Regex.Split(originalText, @"\W+").Where(w => !string.IsNullOrEmpty(w)).ToArray();
        var filteredWords = words.Where(w => !excludedWords.Any(ew => string.Equals(ew, w, StringComparison.OrdinalIgnoreCase)) && !Regex.IsMatch(w, @"^\d+(\.\d+)?$")).ToArray();
        wordCount = filteredWords.Length;

        // самое короткое слово
        shortestWord = filteredWords.OrderBy(w => w.Length).FirstOrDefault();

        // самое длинное слово
        longestWord = filteredWords.OrderByDescending(w => w.Length).FirstOrDefault();

        // подсчёт предложений
        sentenceCount = Regex.Matches(originalText, @"[.!?]").Count;

        // подсчёт гласных и согласных
        glasCount = originalText.Count(c => "aeiouyаеёиоуыэюя".Contains(char.ToLower(c)));
        soglCount = originalText.Count(c => char.IsLetter(c) && !"aeiouyаеёиоуыэюя".Contains(char.ToLower(c)));

        // частота букв
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
                Console.WriteLine("Введите текст (минимум 100 символов):");
                string input = Console.ReadLine();

                if (input.Length < 100)
                {
                    Console.WriteLine("Текст слишком короткий. Попробуйте снова.");
                    continue;
                }

                TextStatistics stats = new TextStatistics(input);
                statisticsList.Add(stats);

                Console.WriteLine("\nСтатистика для введённого текста:");
                Console.WriteLine($"Количество слов (исключая союзы и числа): {stats.wordCount}");
                Console.WriteLine($"Самое короткое слово: {stats.shortestWord}");
                Console.WriteLine($"Количество предложений: {stats.sentenceCount}");
                Console.WriteLine($"Гласных букв: {stats.glasCount}");
                Console.WriteLine($"Согласных букв: {stats.soglCount}");
                Console.WriteLine($"Самое длинное слово: {stats.longestWord}");
                Console.WriteLine("Частота букв:");
                foreach (var pair in stats.letterFrequency.OrderBy(p => p.Key))
                {
                    Console.WriteLine($"  {pair.Key}: {pair.Value}");
                }

                // Предложение удалить буквы
                Console.WriteLine("\nХотите удалить буквы из текста и пересчитать статистику? (y/n)");
                string removeChoice = Console.ReadLine();
                if (removeChoice.ToLower() == "y")
                {
                    Console.WriteLine("Введите буквы через запятую (без пробелов, например: а,б,в):");
                    string lettersInput = Console.ReadLine();
                    var lettersToRemove = lettersInput.Split(',').Select(c => c.Trim().ToLower()).ToList();

                    // Удаление букв из текста
                    string modifiedText = new string(input.Where(c => !lettersToRemove.Contains(char.ToLower(c).ToString())).ToArray());
                    TextStatistics modifiedStats = new TextStatistics(modifiedText);
                    statisticsList.Add(modifiedStats);

                    Console.WriteLine("\nСтатистика после удаления букв:");
                    Console.WriteLine($"Количество слов (исключая союзы и числа): {modifiedStats.wordCount}");
                    Console.WriteLine($"Самое короткое слово: {modifiedStats.shortestWord}");
                    Console.WriteLine($"Количество предложений: {modifiedStats.sentenceCount}");
                    Console.WriteLine($"Гласных букв: {modifiedStats.glasCount}");
                    Console.WriteLine($"Согласных букв: {modifiedStats.soglCount}");
                    Console.WriteLine($"Самое длинное слово: {modifiedStats.longestWord}");
                    Console.WriteLine("Частота букв:");
                    foreach (var pair in modifiedStats.letterFrequency.OrderBy(p => p.Key))
                    {
                        Console.WriteLine($"  {pair.Key}: {pair.Value}");
                    }
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
                        Console.WriteLine($"Количество слов (исключая союзы и числа): {statisticsList[i].wordCount}");
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