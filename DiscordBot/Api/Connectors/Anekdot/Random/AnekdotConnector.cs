using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Api.Connectors.Contracts;
using DiscordBot.Api.Connectors.Contracts.Anekdot.Random.Request;
using DiscordBot.Api.Connectors.Contracts.Anekdot.Random.Response;
using HtmlAgilityPack;

namespace DiscordBot.Api.Connectors.Anekdot.Random
{
    public class AnekdotConnector : ConnectorBase<RandomAnekdotRequest, RandomAnekdotResponse>
    {
        private const string Address = @"https://nekdo.ru/random";
        
        public override async Task<ApiResult<RandomAnekdotResponse>> Invoke(RandomAnekdotRequest request)
        {
            var result = new ApiResult<RandomAnekdotResponse>();
            try
            {
                List<string> anekdots = await ParseAnekdots();

                if (!anekdots.Any())
                {
                    result.IsSuccessful = false;
                    result.Error = "Ничего не найдено";
                }
                else
                {
                    result.IsSuccessful = true;
                    result.Response = new RandomAnekdotResponse
                    {
                        AnekdotText = GetRandomAnekdot(anekdots)
                    };
                }
            }
            catch (Exception e)
            {
                result.IsSuccessful = false;
                result.Error = e.Message;
            }
            
            return result;
        }

        private static string GetRandomAnekdot(List<string> anekdots)
        {
            var random = new System.Random();

            return anekdots[random.Next(0, anekdots.Count)];
        }

        private static async Task<List<string>> ParseAnekdots()
        {
            string htmlPage = await LoadPage();

            if (string.IsNullOrEmpty(htmlPage))
            {
                return new List<string>();
            }

            var document = new HtmlDocument();
            
            document.LoadHtml(htmlPage);

            return document.DocumentNode
                .SelectNodes(xpath: "//div[contains(@class, 'text')]")
                .Select(x => x.InnerText)
                .ToList();
        }

        private static async Task<string> LoadPage()
        {
            var request = (HttpWebRequest)WebRequest.Create(Address);
            var response = (HttpWebResponse)request.GetResponse();
            
            var result = string.Empty;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return result;
            }

            var receiveStream = response.GetResponseStream();
                
            if (receiveStream != null)
            {
                var readStream = response.CharacterSet == null
                    ? new StreamReader(receiveStream)
                    : new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                result = await readStream.ReadToEndAsync().ConfigureAwait(false);
                    
                readStream.Close();
            }
            
            response.Close();

            return result;
        }
    }
}