using Newtonsoft.Json;

namespace DiscordBot.Api
{
    public class Configuration
    {
        [JsonProperty("gifApiKey")]
        public string GifApiKey { get; set; }
    }
}