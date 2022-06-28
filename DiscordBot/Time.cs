using System.Collections.Generic;

namespace DiscordBot
{
    public class Time
    {
        public string NameTime { get; set; }
        public List<Subject> Subjects { get; set; }

        public Time(string nameTime)
        {
            NameTime = nameTime;
            Subjects = new List<Subject>();
        }
    }
}
