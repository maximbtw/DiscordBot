using System.Threading.Tasks;

namespace DiscordBot.Api.Services
{
    public interface IApiHandler<in TParameters, TResult>
    {
        Task<TResult> Invoke(TParameters parameters);
    }
}