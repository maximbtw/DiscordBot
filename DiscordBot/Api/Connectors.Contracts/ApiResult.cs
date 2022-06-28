namespace DiscordBot.Api.Connectors.Contracts
{
    public class ApiResult<TApiResponse>
    {
        public TApiResponse Response { get; set; }
        
        public string Error { get; set; }
        
        public bool IsSuccessful { get; set; }
    }
}