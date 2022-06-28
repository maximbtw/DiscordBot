using System;
using System.Collections.Generic;

namespace DiscordBot
{
    public class DayOfTheWeak
    {
        public string NameDay { get; }
        public List<Time> Times { get; }
        public DayOfTheWeak(string nameDay)
        {
            NameDay = nameDay;
            Times = new List<Time>(5);
        }

        public static Tuple<string, string> GetDayOfTheWeak(string text)
        {
            var date = DateTime.Today.Date;

            switch (text)
            {
                case ("завтра"):
                    date = date.AddDays(1);
                    break;
                case ("сегодня"):
                    break;
                case ("вчера"):
                    date = date.AddDays(6);
                    break;
                default:
                    return null;
            }
            var day = ConvertToRus(date.DayOfWeek.ToString());
            var weak = Weak.GetWeak();

            if (day.Equals("суббота") || day.Equals("воскресенье"))
            {
                day = "Выходной";
            }
            if (day.Equals("понедельник") && text.Equals("завтра"))
            {
                weak = Weak.GetNextWeak();
            }

            return Tuple.Create(day, weak);
        }

        private static string ConvertToRus(string day)
        {
            switch (day.ToLower())
            {
                case "monday":
                    day = "понедельник";
                break;
                case "tuesday":
                    day = "вторник";
                    break;
                case "wednesday":
                    day = "среда";
                    break;
                case "thursday":
                    day = "четверг";
                    break;
                case "friday":
                    day = "пятница";
                    break;
                case "saturday":
                    day = "суббота";
                    break;
                case "sunday":
                    day = "воскресенье";
                    break;
                default:
                    day = string.Empty;
                    break;
            }
            return day;
        }
    }
}
