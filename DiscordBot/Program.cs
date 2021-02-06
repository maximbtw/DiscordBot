

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Anekdot.LoadAnikdots();

            var bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}
