using System.Threading.Tasks;
using DiscordBot.Api.Services.Contracts.Anekdot.Parameters;
using DiscordBot.Api.Services.Contracts.Anekdot.Result;
using DiscordBot.Api.Services.Contracts.Gif.Translate.Parameters;
using DiscordBot.Api.Services.Contracts.Gif.Translate.Result;

namespace DiscordBot.Api.Services
{
    public interface IApiService
    {
        Task<GifApiTranslateResult> GifTranslate(GifApiTranslateParameters translateParameters);
        
        Task<ApiAnekdotResult> GetRandomAnekdot(ApiAnekdotParameters translateParameters);
    }
}