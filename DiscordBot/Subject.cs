using System;

namespace DiscordExcel
{
    public class Subject
    {
        public string TextSubject { get; set; }
        public Weak OddWeak { get; set; }
        public Weak EvenWeak { get; set; }

        public Subject()
        {
            OddWeak = new Weak("Нечётная", true);
            EvenWeak = new Weak("Четная", true);
        }

        public string GetWeak()
        {
            if(OddWeak.ContainsSubject && EvenWeak.ContainsSubject)
            {
                return "Любая";
            }
            if (OddWeak.ContainsSubject) return OddWeak.NameWeak;
            return EvenWeak.NameWeak;       
        }

        public bool ContainsInWeak(string weak)
        {
            if (weak.ToLower().Equals("чётная"))
            {
                return EvenWeak.ContainsSubject;
            }
            else if (weak.ToLower().Equals("нечётная"))
            {
                return OddWeak.ContainsSubject;
            }
            else if (weak.ToLower().Equals("любая"))
            {
                return OddWeak.ContainsSubject || EvenWeak.ContainsSubject;
            }

            throw new ArgumentException("название недели не точное"); 
        }
    }
}
