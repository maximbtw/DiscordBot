using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("config.json"))
                using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                    json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var confogJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = confogJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,  
                //LogLevel = LogLevel.Debug,
                //UseInternalLogHandler = true,       
            };

            Client = new DiscordClient(config); 

            Client.Ready += OnClientReady;

            var comandsConfig = new CommandsNextConfiguration 
            { 
                StringPrefixes = new string[] { confogJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                //DmHelp = true,
            };

            Commands = Client.UseCommandsNext(comandsConfig);
            Commands.RegisterCommands<BotCommand>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
