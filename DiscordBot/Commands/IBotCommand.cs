using System.Threading.Tasks;
using DSharpPlus.CommandsNext;

namespace DiscordBot.Commands
{
    public interface IBotCommand
    {
        Task GetTranslate(CommandContext ctx, params string[] text);
        
        Task GetRandomAnekdot(CommandContext ctx);
    }
}