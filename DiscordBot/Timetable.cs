using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiscordExcel;

namespace DiscordBot
{
    public class Timetable
    {
        public List<DayOfTheWeak> DayOfWeeks { get; }
        
        private readonly string _groupName;

        public Timetable(string groupName)
        {
            _groupName = groupName;
            DayOfWeeks = new List<DayOfTheWeak>(5);
        }

        public static IEnumerable<string> GetMessageTimetable(string groupName, Tuple<string,string> dayAndWeak = null)
        {
            var parser = new ExcelParser(groupName.ToLower());

            if (parser.TryWriteExcelDocument())
            {
                parser.Parse();

                IEnumerable<string> text = parser.Timetable.CreateTextToDisplay(dayAndWeak);
                
                foreach(string lineText in text)
                {
                    yield return lineText;
                }
            }
            else
            {
                string message = $"\"{groupName}\" - такой группы не найдено";
                
                yield return message;
            }
        }

        private IEnumerable<string> CreateTextToDisplay(Tuple<string, string> dayAndWeak = null)
        {
            string title = $"Расписание для группы: {_groupName.ToUpper()}\n";

            yield return title;
            
            var textBuilder = new StringBuilder();

            foreach (DayOfTheWeak dayOfWeak in DayOfWeeks)
            {
                bool isNotCurrentDay =
                    dayAndWeak != null && !dayAndWeak.Item1.ToLower().Equals(dayOfWeak.NameDay.ToLower());
                
                if (isNotCurrentDay)
                {
                    continue;
                }

                textBuilder.Append($"--------------------------------------------------------------");
                textBuilder.Append($"\n{dayOfWeak.NameDay}");

                foreach (Time time in dayOfWeak.Times)
                {
                    bool containsTime =
                        dayAndWeak == null || time.Subjects.Any(x => x.ContainsInWeak(dayAndWeak.Item2));
                    
                    if (!containsTime)
                    {
                        continue;
                    }

                    textBuilder.Append($"\n\n [{time.NameTime }]\n");

                    foreach (Subject subject in time.Subjects)
                    {
                        bool containsWeek = dayAndWeak == null || subject.ContainsInWeak(dayAndWeak.Item2);
                        
                        if (containsWeek)
                        {
                            continue;
                        }

                        textBuilder.Append($"\n[Неделя: {subject.GetWeak()}]");
                        textBuilder.Append($"\n{subject.TextSubject}");
                    }
                }
                
                yield return textBuilder.ToString();
                
                textBuilder.Clear();
            }
        }
    }
}
