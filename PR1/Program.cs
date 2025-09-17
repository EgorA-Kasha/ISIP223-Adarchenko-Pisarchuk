using System.Text.RegularExpressions;

class Program
{
    class Text
    {
        public string originalText {  get; set; }
        public string wordCount { get; set; }
        public string shortestWord { get; set; }
        public string longestWord { get; set; }
        public int sentenceCount { get; set; }
        public int glasCount { get; set; }
        public int soglCount { get; set; }
        public Dictionary<char, int> letterFrequency { get; set; } = new Dictionary<char, int>();
    }

    static void Main(string[] args)
    {
        List<TextStats> history = new List<TextStats>();

        while (true)
        {
            Console.WriteLine("Введите текст (минимум 100 символов");
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input) || input.Length < 100)
            {
                Console.WriteLine("Ошибка. Вводмимый текст должен содержать БОЛЬШЕ 100 символов.\n");
                continue;
            }

            TextStats stats = new TextStats { originalText = input };

            string[] words = Regex.Split(input, @"\s+").Where(w => !string.IsNullOrEmpty(w)).ToArray();

            stats.wordCount = words.Length;

            string[] cleanWords = words.Select(w => Regex.Replace(w, "[^\p{L}]", "")).Where(w => !string.IsNullOrEmpty(w)).ToArray();
            if (cleanWords.Length > 0) {
                stats.shortestWord = cleanWords.OrderBy(w => w.Length).First();
                stats.longestWord = cleanWords.OrderByDescending(w => w.Length).First();
            }

            string[] sentences = Regex.Split(input, @"(?<!\w\.\w.)(?<![A-Z][a-z]\.)(?<=\.|\?|\!)(\s|$)");

            string glas = "аеёиоуыэюяАЕЁИОУЫЭЮЯ";

            string sogl = "бвгджзйклмнпрстфхцчшщъьБВГДЖЗЙКЛМНПРСТФХЦЧШЩЪЬ";

            int glasCount = 0;
            int soglCount = 0;


            }

    }
}



