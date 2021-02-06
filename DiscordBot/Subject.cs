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
    }
}
