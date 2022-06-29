using System.Threading.Tasks;
using DiscordBot.Api.Services.Contracts.Anekdot.Parameters;
using DiscordBot.Api.Services.Contracts.Anekdot.Result;
using DiscordBot.Api.Services.Contracts.Gif.Translate.Parameters;
using DiscordBot.Api.Services.Contracts.Gif.Translate.Result;
using DiscordBot.Api.Services.Operations.Anekdot.Random;
using DiscordBot.Api.Services.Operations.Gif.Translate;

namespace DiscordBot.Api.Services
{
    public class ApiService : ServiceBase, IApiService
    {
        private readonly GifTranslateHandler _gifApiTranslateHandler;
        private readonly RandomAnekdotHandler _randomAnekdotHandler;

        public ApiService()
        {
            _gifApiTranslateHandler = new GifTranslateHandler(Configuration);
            _randomAnekdotHandler = new RandomAnekdotHandler(Configuration);
        }

        public async Task<GifApiTranslateResult> GifTranslate(GifApiTranslateParameters parameters) =>
            await _gifApiTranslateHandler.Invoke(parameters);

        public async Task<ApiAnekdotResult> GetRandomAnekdot(ApiAnekdotParameters parameters) =>
            await _randomAnekdotHandler.Invoke(parameters);
    }
}