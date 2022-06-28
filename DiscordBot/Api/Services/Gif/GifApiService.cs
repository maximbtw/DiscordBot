using System.Threading.Tasks;
using DiscordBot.Api.Services.Contracts.Gif.Translate.Parameters;
using DiscordBot.Api.Services.Contracts.Gif.Translate.Result;
using DiscordBot.Api.Services.Gif.Translate;


namespace DiscordBot.Api.Services.Gif
{
    public class GifApiService : ServiceBase, IGifService
    {
        private readonly IApiHandler<GifApiTranslateParameters, GifApiTranslateResult> _gifApiTranslateHandler;

        public GifApiService()
        {
            _gifApiTranslateHandler = new GifTranslateHandler(Configuration);
        }

        public async Task<GifApiTranslateResult> GifTranslate(GifApiTranslateParameters parameters) =>
            await _gifApiTranslateHandler.Invoke(parameters);
    }
}