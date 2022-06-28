namespace DiscordBot.Api.Services.Contracts.Gif.Translate.Result
{
    public class GifApiTranslateResult 
    {
        public bool IsSuccessful { get; set; }
        
        public string Url { get; set; }
        
        public string Title { get; set; }
    }
}