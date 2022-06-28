using Newtonsoft.Json;

namespace DiscordBot.Api.Connectors.Contracts.Gif.Translate.Response
{
    public class GifTranslateResponse
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
        
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }
    
    public class Data
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class Meta
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        
        [JsonProperty("msg")]
        public string Message { get; set; }
        
        [JsonProperty("response_id")]
        public string ResponseId { get; set; }
    }
}