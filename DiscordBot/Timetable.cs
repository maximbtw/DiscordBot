using System.Collections.Generic;
using System.Text;

namespace DiscordExcel
{
    public class Timetable
    {
        public string Group { get; set; }
        public List<DayOfTheWeak> dayOfWeeks { get; set; }

        public Timetable(string group)
        {
            Group = group;
            dayOfWeeks = new List<DayOfTheWeak>(5);
        }

        public static string GetMessageTimetable(string groupName, string _dayOfWeak = null)
        {
            var parser = new ExcelParser();

            return (!parser.WritingExcel(groupName.ToLower()))
                    ? $"\"{groupName}\" - такой группы не найдено"
                    : parser.Timetable.FormatMesssage(_dayOfWeak);
        }

        private string FormatMesssage(string _dayOfWeak = null)
        {
            var weak = string.IsNullOrEmpty(_dayOfWeak) ? string.Empty : Weak.GetWeak().Split(' ')[1].ToLower();

            var message = new StringBuilder($"Расписание для группы: {Group.ToUpper()}\n");

            foreach (var dayOfWeak in dayOfWeeks)
            {
                if (!string.IsNullOrEmpty(_dayOfWeak)
                 && !_dayOfWeak.Equals(dayOfWeak.NameDay.ToLower()))
                    continue;
                message.Append($"\n{dayOfWeak.NameDay}\n");

                foreach (var time in dayOfWeak.Times)
                {
                    if (CheckTime(weak, time)) continue;
                    message.Append($"\n {time.NameTime }");

                    foreach (var subject in time.Subjects)
                    {
                        if (SignificanceWeak(weak, subject)) continue;

                        if (subject.EvenWeak.ContainsSubject
                        && !subject.OddWeak.ContainsSubject
                        && string.IsNullOrEmpty(weak))
                            message.Append($"\n {subject.EvenWeak.NameWeak }");

                        if (!subject.EvenWeak.ContainsSubject
                          && subject.OddWeak.ContainsSubject
                          && string.IsNullOrEmpty(weak))
                            message.Append($"\n {subject.OddWeak.NameWeak }");

                        message.Append($"\n {subject.TextSubject}\n");
                    }
                }
            }

            return message.ToString();
        }

        private bool SignificanceWeak(string weak, Subject subject)
        {
            return !string.IsNullOrEmpty(weak) 
               && ((weak.Equals(subject.EvenWeak.NameWeak.ToLower()) && !subject.EvenWeak.ContainsSubject)
                || (weak.Equals(subject.OddWeak.NameWeak.ToLower()) && !subject.OddWeak.ContainsSubject));
        }

        private bool CheckTime(string weak, Time time)
        {
            foreach(var subject in time.Subjects)
                if (!SignificanceWeak(weak, subject)) 
                    return false; 
            return true;
        }
    }
}
