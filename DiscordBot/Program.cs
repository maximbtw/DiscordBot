

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Anekdot.Load();

            var bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}
