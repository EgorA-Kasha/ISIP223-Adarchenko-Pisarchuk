using System.Text.RegularExpressions;

class TextStatistics
{
    public string OriginalText { get; set; }
    public int WordCount { get; set; }
    public string ShortestWord { get; set; }
    public int SentenceCount { get; set; }
    public int VowelCount { get; set; }
    public int ConsonantCount { get; set; }
    public string LongestWord { get; set; }
    public Dictionary<char, int> LetterFrequency { get; set; }

    public TextStatistics(string text)
    {
        OriginalText = text;
        AnalyzeText();
    }

    private void AnalyzeText()
    {
        // Подсчёт слов: разделяем по пробелам и знакам препинания
        var words = Regex.Split(OriginalText, @"\W+").Where(w => !string.IsNullOrEmpty(w)).ToArray();
        WordCount = words.Length;

        // Самое короткое слово
        ShortestWord = words.OrderBy(w => w.Length).FirstOrDefault();

        // Самое длинное слово
        LongestWord = words.OrderByDescending(w => w.Length).FirstOrDefault();

        // Подсчёт предложений: по точкам, восклицательным и вопросительным знакам
        SentenceCount = Regex.Matches(OriginalText, @"[.!?]").Count;

        // Подсчёт гласных и согласных (для английского и русского)
        VowelCount = OriginalText.Count(c => "aeiouyаеёиоуыэюя".Contains(char.ToLower(c)));
        ConsonantCount = OriginalText.Count(c => char.IsLetter(c) && !"aeiouyаеёиоуыэюя".Contains(char.ToLower(c)));

        // Частота букв
        LetterFrequency = new Dictionary<char, int>();
        foreach (char c in OriginalText.Where(char.IsLetter))
        {
            char lower = char.ToLower(c);
            if (LetterFrequency.ContainsKey(lower))
                LetterFrequency[lower]++;
            else
                LetterFrequency[lower] = 1;
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
            Console.WriteLine($"Количество слов: {stats.WordCount}");
            Console.WriteLine($"Самое короткое слово: {stats.ShortestWord}");
            Console.WriteLine($"Количество предложений: {stats.SentenceCount}");
            Console.WriteLine($"Гласных букв: {stats.VowelCount}");
            Console.WriteLine($"Согласных букв: {stats.ConsonantCount}");
            Console.WriteLine($"Самое длинное слово: {stats.LongestWord}");
            Console.WriteLine("Частота букв:");
            foreach (var pair in stats.LetterFrequency.OrderBy(p => p.Key))
            {
                Console.WriteLine($"  {pair.Key}: {pair.Value}");
            }

            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Продолжить с новым текстом");
            Console.WriteLine("2. Выйти");

            string choice = Console.ReadLine();
            if (choice == "1")
            {
                continue;
            }
            
            else if (choice == "2")
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
