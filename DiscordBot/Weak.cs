using System;

namespace DiscordExcel
{
    public class Weak
    {
        public string NameWeak { get; set; }
        public bool ContainsSubject { get; set; }

        public Weak(string nameWeak, bool contains)
        {
            NameWeak = nameWeak;
            ContainsSubject = contains;
        }

        public static string GetWeak()
        {
            var startDay = new DateTime(2021, 02, 01);
            var toDay = DateTime.Today;

            var countDays = toDay.Subtract(startDay).Days;
            var count = 0;
            while (countDays >= 0)
            {
                count++;
                countDays -= 7;
            }

            var message = $"№{count} ";
            message += (count % 2 == 0) ? "Чётная" : "Нечётная";
            return message;
        }
    }
}
