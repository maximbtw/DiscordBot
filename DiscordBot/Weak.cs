using System;

namespace DiscordExcel
{
    public class Weak
    {
        private static readonly DateTime startDay = new DateTime(2021, 02, 01);
        public string NameWeak { get; set; }
        public bool ContainsSubject { get; set; }

        public Weak(string nameWeak, bool contains)
        {
            NameWeak = nameWeak;
            ContainsSubject = contains;
        }

        public static int GetAge(DateTime data) => DateTime.Today.Subtract(data).Days;

        public static string GetNextWeak() => (GetNumberOfWeak() % 2 != 0) ? "Чётная" : "Нечётная";

        public static string GetWeak() => (GetNumberOfWeak() % 2 == 0) ? "Чётная" : "Нечётная";

        public static int GetNumberOfWeak()
        {
            var countDays = DateTime.Today.Subtract(startDay).Days;
            var count = 0;
            while (countDays >= 0)
            {
                count++;
                countDays -= 7;
            }

            return count;
        }
    }
}
