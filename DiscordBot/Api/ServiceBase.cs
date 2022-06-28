using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace DiscordBot.Api
{
    public class ServiceBase : IDisposable
    {
        public Configuration Configuration { get; }
        
        private readonly Component _component = new Component();
        private bool _disposed;

        protected ServiceBase()
        {
            Configuration = LoadConfiguration();
        }
        
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private Configuration LoadConfiguration()
        {
            string configPath = "config.json";

            using (var file = File.OpenRead(configPath))
            {
                using (var streamReader = new StreamReader(file, new UTF8Encoding(false)))
                {
                    string json = streamReader.ReadToEnd();

                    return JsonConvert.DeserializeObject<Configuration>(json);
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(disposing)
                {
                    _component.Dispose();
                }
                
                _disposed = true;
            }
        }
        
        ~ServiceBase()
        {
            Dispose(disposing: false);
        }
    }
}