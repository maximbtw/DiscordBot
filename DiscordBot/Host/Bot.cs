using System.IO;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;

namespace DiscordBot.Host
{
    internal class Bot
    {
        private CommandsNextExtension _commands;
        private DiscordClient _client;
        
        public async Task RunAsync()
        {
            const string configPath = "config.json";
            
            string json;

            await using (var file = File.OpenRead(configPath))
            {
                using (var streamReader = new StreamReader(file, new UTF8Encoding(false)))
                {
                    json = await streamReader.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false);
                }
            }

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
            };

            _client = new DiscordClient(config); 

            _client.Ready += OnClientReady;

            var commandsConfig = new CommandsNextConfiguration 
            { 
                StringPrefixes = new[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
            };

            _commands = _client.UseCommandsNext(commandsConfig);
            _commands.RegisterCommands<BotCommand>();

            await _client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
