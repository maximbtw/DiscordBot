using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Api.Services;
using DiscordBot.Api.Services.Contracts.Anekdot.Parameters;
using DiscordBot.Api.Services.Contracts.Gif.Translate.Parameters;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    public static class BotCommandManager
    {
        /// <summary>
        /// Возвращает рандомное число в диапозоне
        /// </summary>
        /// <param name="minValue">Минимальное число</param>
        /// <param name="maxValue">Максимальное число</param>
        /// <param name="countRolls">Количесво реролов (изменени числа в сообщении)</param>
        public static async Task Roll(CommandContext ctx, int minValue = 0, int maxValue = 100, int countRolls = 5)
        {
            var randomNumber = Helpers.GetRandom(minValue, maxValue + 1).ToString();

            DiscordMessage message = await ctx.RespondAsync(randomNumber);
            
            for (int i = 0; i < countRolls; i++)
            {
                randomNumber = Helpers.GetRandom(minValue, maxValue + 1).ToString();
                
                await message.ModifyAsync(randomNumber);
                
                System.Threading.Thread.Sleep(millisecondsTimeout: 100);
            }
        }

        /// <summary>
        /// Возвращает список, рандомно отсортированых пользователей из голосового чата.
        /// </summary>
        /// <param name="ctx"></param>
        public static async Task RollWithLobbyUsers(CommandContext ctx)
        {
            string[] userNickNames = ctx.Member?.VoiceState?
                .Channel.Users
                .Select(x => x.DisplayName)
                .ToArray();
            
            string message = string.Empty;
            
            if (Helpers.CollectionIsNullOrEmpty(userNickNames))
            {
                message = "Users not found";
            }
            else
            {
                Helpers.ShufleArray(userNickNames);

                for (int i = 0; i < userNickNames!.Length; i++)
                {
                    message += i + 1 + ": " + userNickNames[i] + "\n";
                }
            }

            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }
            
        
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