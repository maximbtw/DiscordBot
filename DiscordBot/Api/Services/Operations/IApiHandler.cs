using System.Threading.Tasks;

namespace DiscordBot.Api.Services.Operations
{
    public abstract class ApiHandlerBase<TParameters, TResult>
    {
        protected readonly Configuration Configuration;

        public ApiHandlerBase(Configuration configuration)
        {
            Configuration = configuration;
        }

        public abstract Task<TResult> Invoke(TParameters parameters);
    }
}