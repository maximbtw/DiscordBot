using System;
using System.Threading.Tasks;
using DiscordBot.Api.Connectors.Contracts;
using Newtonsoft.Json;
using RestSharp;

namespace DiscordBot.Api.Connectors
{
    public abstract class ConnectorBase<TRequest, TResponse> where TResponse : class
    {
        public abstract Task<ApiResult<TResponse>> Invoke(TRequest request);

        protected async Task<ApiResult<TResponse>> InvokeToExternalSystem(string address, string url)
        {
            var options = new RestClientOptions(address)
            {
                ThrowOnAnyError = true,
                MaxTimeout = 1000
            };

            var client = new RestClient(options);

            RestResponse response = await client.GetAsync(new RestRequest(url));

            return ConvertToApiResult(response);
        }

        private static ApiResult<TResponse> ConvertToApiResult(RestResponseBase response)
        {
            if (!response.IsSuccessful)
            {
                return new ApiResult<TResponse>
                {
                    Error = response.ErrorMessage,
                    IsSuccessful = false
                };
            }

            bool converted = TryConvertWrappedResponse(response.Content, out TResponse apiResponse);

            if (converted)
            {
                return new ApiResult<TResponse>
                {
                    Response = apiResponse,
                    IsSuccessful = true
                };
            }
                
            return new ApiResult<TResponse>
            {
                Error = "Result not converted",
                IsSuccessful = false
            };
        }

        private static bool TryConvertWrappedResponse(string wrappedResult, out TResponse response)
        {
            response = null;

            if (wrappedResult == null)
            {
                return false;
            }

            try
            {
                response = JsonConvert.DeserializeObject<TResponse>(wrappedResult);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}