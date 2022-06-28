using System.Threading.Tasks;
using DiscordBot.Api.Services.Contracts.Gif.Translate.Parameters;
using DiscordBot.Api.Services.Contracts.Gif.Translate.Result;

namespace DiscordBot.Api.Services.Gif
{
    public interface IGifService
    {
        Task<GifApiTranslateResult> GifTranslate(GifApiTranslateParameters translateParameters);
    }
}