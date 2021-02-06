using System;
using System.Collections.Generic;

namespace DiscordExcel
{
    public class DayOfTheWeak
    {
        public string NameDay { get; set; }
        public List<Time> Times { get; set; }
        public DayOfTheWeak(string nameDay)
        {
            NameDay = nameDay;
            Times = new List<Time>(5);
        }

        public static string GetDayOfTheWeak(string text)
        {
            var date = DateTime.Today.Date;

            switch (text)
            {
                case ("завтра"):
                    date = date.AddDays(1);
                    break;
                case ("вчера"):
                    date = date.AddDays(6);
                    break;
            }
            var message = ConvertToRus(date.DayOfWeek.ToString());

            if (message.Equals("суббота") || message.Equals("воскресенье"))
                message = "Выходной";

            return message;
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
