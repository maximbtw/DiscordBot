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
            var message = string.Empty;

            switch (text)
            {
                case ("завтра"):
                    date = date.AddDays(1);
                    message = date.ToString("dddd");
                    break;
                case ("сегодня"):
                    message = date.ToString("dddd");
                    break;
                case ("вчера"):
                    date = date.AddDays(6);
                    message = date.ToString("dddd");
                    break;
            }

            if (message.Equals("суббота") || message.Equals("воскресенье"))
                message = "Выходной";

            return message;
        }
    }
}
