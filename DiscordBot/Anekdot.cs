using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiscordBot
{
    public class Anekdot
    {
        public static List<string> Anekdots;

        public static void LoadAnikdots()
        {
            string path = "aneks.sql";

            Anekdots = File.ReadAllLines(path)
                .Where(x => x.IndexOf('\'') >= 0)
                .Select(x => x.Substring(x.IndexOf('\'') + 1))
                .Select(x => x.Substring(0, x.Length - 3))
                .Select(x => x.Replace("\\n", "\n"))
                .Select(x => x.Replace("&quot;", ""))
                .Select(x => x.Replace("\"\"", "\""))
                .ToList();
        }
    }
}
