using System.Threading.Tasks;
using DiscordBot.Api.Connectors.Anekdot.Random;
using DiscordBot.Api.Connectors.Contracts.Anekdot.Random.Request;
using DiscordBot.Api.Services.Contracts.Anekdot.Parameters;
using DiscordBot.Api.Services.Contracts.Anekdot.Result;

namespace DiscordBot.Api.Services.Operations.Anekdot.Random
{
    public class RandomAnekdotHandler : ApiHandlerBase<ApiAnekdotParameters, ApiAnekdotResult>
    {
        public RandomAnekdotHandler(Configuration configuration) : base(configuration)
        {
        }

        public override async Task<ApiAnekdotResult> Invoke(ApiAnekdotParameters parameters)
        {
            var connector = new AnekdotConnector();

            var request = new RandomAnekdotRequest();

            var apiResult = await connector.Invoke(request);

            if (!apiResult.IsSuccessful)
            {
                return new ApiAnekdotResult
                {
                    IsSuccessful = false
                };
            }

            int anekdotIndex = Helpers.GetRandom(0, apiResult.Response.Anekdots.Count);
            
            return new ApiAnekdotResult
            {
                IsSuccessful = true,
                Text = apiResult.Response.Anekdots[anekdotIndex]
            };
        }
    }
}