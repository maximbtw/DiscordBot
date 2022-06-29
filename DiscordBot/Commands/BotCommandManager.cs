using System.Text;
using System.Threading.Tasks;
using DiscordBot.Api.Services;
using DiscordBot.Api.Services.Contracts.Anekdot.Parameters;
using DiscordBot.Api.Services.Contracts.Gif.Translate.Parameters;
using DSharpPlus.CommandsNext;

namespace DiscordBot.Commands
{
    public class BotCommandManager
    {
        /// <summary>
        /// Возвращает gif по тексту.
        /// </summary>
        public static async Task GetTranslateGif(CommandContext ctx, params string[] text)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string letter in text)
            {
                builder.Append(letter);
            }

            var parameters = new GifApiTranslateParameters
            {
                TextToTranslate = builder.ToString()
            };

            // ReSharper disable once ConvertToUsingDeclaration
            using (var service = new ApiService())
            {
                var result = await service.GifTranslate(parameters);
                
                if (result.IsSuccessful)
                {
                    await ctx.Channel.SendMessageAsync(result.Url).ConfigureAwait(false);
                }
                else
                {
                    await ctx.Channel.SendMessageAsync("Ох.. Не смог ничего найти...").ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Возвращает рандомный анекдот.
        /// </summary>
        public static async Task GetRandomAnekdot(CommandContext ctx)
        {
            var parameters = new ApiAnekdotParameters();

            // ReSharper disable once ConvertToUsingDeclaration
            using (var service = new ApiService())
            {
                var result = await service.GetRandomAnekdot(parameters);
                
                if (result.IsSuccessful)
                {
                    await ctx.Channel.SendMessageAsync(result.Text).ConfigureAwait(false);
                }
                else
                {
                    await ctx.Channel.SendMessageAsync("Ох.. Что-то пошло не так...").ConfigureAwait(false);
                }
            }
        }
    }
}