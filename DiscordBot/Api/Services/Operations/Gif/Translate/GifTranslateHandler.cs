using System.Threading.Tasks;
using DiscordBot.Api.Connectors.Contracts.Gif.Translate.Request;
using DiscordBot.Api.Connectors.Gif.Translate;
using DiscordBot.Api.Services.Contracts.Gif.Translate.Parameters;
using DiscordBot.Api.Services.Contracts.Gif.Translate.Result;

namespace DiscordBot.Api.Services.Operations.Gif.Translate
{
    public class GifTranslateHandler : ApiHandlerBase<GifApiTranslateParameters, GifApiTranslateResult>
    {
        public GifTranslateHandler(Configuration configuration) : base(configuration)
        {
        }
        
        public override async Task<GifApiTranslateResult> Invoke(GifApiTranslateParameters parameters)
        {
            var connector = new GifTranslateConnector();

            GifTranslateRequest request = new GifTranslateRequest()
            {
                ApiKey = Configuration.GifApiKey,
                Text = parameters.TextToTranslate
            };

            var apiResult = await connector.Invoke(request);

            if (!apiResult.IsSuccessful)
            {
                return new GifApiTranslateResult
                {
                    IsSuccessful = false
                };
            }
            
            return new GifApiTranslateResult
            {
                IsSuccessful = true,
                Title = apiResult.Response.Data.Title,
                Url = apiResult.Response.Data.Url
            };
        }
    }
}