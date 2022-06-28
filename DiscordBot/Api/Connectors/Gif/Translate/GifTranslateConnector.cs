using System.Threading.Tasks;
using DiscordBot.Api.Connectors.Contracts;
using DiscordBot.Api.Connectors.Contracts.Gif.Translate.Request;
using DiscordBot.Api.Connectors.Contracts.Gif.Translate.Response;
using Flurl;

namespace DiscordBot.Api.Connectors.Gif.Translate
{
    public class GifTranslateConnector : ConnectorBase<GifTranslateRequest, GifTranslateResponse>
    {
        private const string Address = @"https://api.giphy.com/v1/gifs/translate";

        public override async Task<ApiResult<GifTranslateResponse>> Invoke(GifTranslateRequest request)
        {
            string url = Address
                .SetQueryParam("api_key", request.ApiKey)
                .SetQueryParam("s", request.Text);

            return await InvokeToExternalSystem(Address, url);
        }
    }
}