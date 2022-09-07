using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DiscordBot.Commands
{
    public class BotCommand : BaseCommandModule, IBotCommand
    {
        [Command("Roll")]
        [Description("Рандомное число число")]
        public async Task Roll(CommandContext ctx,
            [Description("Минимальное значаение")]  int minValue = 0,
            [Description("Максимальное значаение")] int maxValue = 100,
            [Description("Количесво реролов")] int countRolls = 5)
        {
            await BotCommandManager.Roll(ctx, minValue, maxValue, countRolls);
        }
        
        [Command("LobbyRoll")]
        [Description("Список пользователей в случайном порядке")]
        public async Task RollWithLobbyUsers(CommandContext ctx) => await BotCommandManager.RollWithLobbyUsers(ctx);

        [Command(name: "Gif")]
        [Description("Возвращает gif по тексту")]
        public async Task GetTranslate(CommandContext ctx, params string[] text) =>
            await BotCommandManager.GetTranslateGif(ctx, text);

        [Command(name: "Анекдот")]
        [Description("Возвращает рандомный анекдот")]
        public async Task GetRandomAnekdot(CommandContext ctx) =>
            await BotCommandManager.GetRandomAnekdot(ctx);
    }
}
