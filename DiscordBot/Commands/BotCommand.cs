using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DiscordBot.Commands
{
    public class BotCommand : BaseCommandModule, IBotCommand
    {
        [Command("Roll")]
        [Description("Рандомит число")]
        public async Task Roll(CommandContext ctx,
            [Description("Минимальное значаение")] int minValue = 0,
            [Description("Максимальное значаение")] int maxValue = 101)
        {
            var number = CommandAsset.GetRandom(minValue, maxValue).ToString();

            var msg1 = await ctx.RespondAsync(number);
            for (int i = 0; i < 5; i++)
            {
                number = CommandAsset.GetRandom(minValue, maxValue).ToString();
                await msg1.ModifyAsync(number);
                System.Threading.Thread.Sleep(100);
            }
        }

        [Command("BigRoll")]
        [Description("Список пользователей в случайном порядке")]
        public async Task RollWithUsers(CommandContext ctx)
        {
            var users = ctx.Member.VoiceState?
                                  .Channel.Users
                                  .Select(x => x.DisplayName)
                                  .ToArray();
            if (users == null) return;
            CommandAsset.ShufleArray(users);

            string text = string.Empty;
            for (int i = 0; i < users.Length; i++)
                text += (i + 1).ToString() + ": " + users[i] + "\n";

            await ctx.Channel.SendMessageAsync(text).ConfigureAwait(false);
        }

        [Command(name: "Gif")]
        [Description("Возвращает gif по тексту")]
        public async Task GetTranslate(CommandContext ctx, params string[] text) =>
            await BotCommandManager.GetTranslateGif(ctx, text);

        [Command(name: "Анедот")]
        [Description("Возвращает рандомный анекдот")]
        public async Task GetRandomAnekdot(CommandContext ctx) =>
            await BotCommandManager.GetRandomAnekdot(ctx);
    }
}
