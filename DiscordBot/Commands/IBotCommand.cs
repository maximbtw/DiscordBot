using System.Threading.Tasks;
using DSharpPlus.CommandsNext;

namespace DiscordBot.Commands
{
    public interface IBotCommand
    {
        Task GetTranslate(CommandContext ctx, params string[] text);
        
        Task GetRandomAnekdot(CommandContext ctx);

        Task Roll(CommandContext ctx, int minValue = 0, int maxValue = 101, int countRolls = 5);
        
        Task RollWithLobbyUsers(CommandContext ctx);
    }
}