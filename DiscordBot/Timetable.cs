using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

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

        public static IEnumerable<string> GetMessageTimetable
            (string groupName, Tuple<string,string> dayAndWeak = null)
        {
            var parser = new ExcelParser();

            var isParsed = parser.WritingExcel(groupName.ToLower());

            if (!isParsed)
            {
                yield return $"\"{groupName}\" - такой группы не найдено";
                yield break;
            }

            foreach(var message in parser.Timetable.CreateMessage(dayAndWeak))
            {
                yield return message;
            }
        }

        private IEnumerable<string> CreateMessage(Tuple<string, string> dayAndWeak = null)
        {
            var weak = (dayAndWeak == null) ? "любая" : dayAndWeak.Item2;
            var message = new StringBuilder($"Расписание для группы: {Group.ToUpper()}\n");
            yield return message.ToString();
            message.Clear();

            foreach (var dayOfWeak in dayOfWeeks)
            {
                if (dayAndWeak != null && !dayAndWeak.Item1.ToLower().Equals(dayOfWeak.NameDay.ToLower()))
                    continue;

                message.Append($"--------------------------------------------------------------");
                message.Append($"\n{dayOfWeak.NameDay}");

                foreach (var time in dayOfWeak.Times)
                {
                    if (time.Subjects.Count == 0) continue;
                    if (dayAndWeak != null && !time.Subjects.Any(x => x.ContainsInWeak(dayAndWeak.Item2))) continue;

                    message.Append($"\n\n [{time.NameTime }]\n");

                    foreach (var subject in time.Subjects)
                    {
                        if (dayAndWeak != null && !subject.ContainsInWeak(dayAndWeak.Item2)) continue;

                        message.Append($"\n[Неделя: {subject.GetWeak()}]");
                        message.Append($"\n{subject.TextSubject}");
                    }
                }
                yield return message.ToString();
                message.Clear();
            }
        }
    }
}
