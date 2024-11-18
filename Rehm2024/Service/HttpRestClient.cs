using Newtonsoft.Json;
using Rehm.Shared;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Markup;

namespace Rehm2024.Service
{
    public class HttpRestClient
    {
        private readonly string apiUrl;
        protected readonly RestClient client;

        public HttpRestClient(string apiUrl)
        {
            this.apiUrl = apiUrl;
            client = new RestClient();
        }

        public async Task<ApiResponse> PostRequstAsync(BaseRequest baseRequest)
        {
            try
            {
                var Restclient = new RestClient();

                //var request = new RestRequest(apiUrl ,baseRequest.Method);

                var request = new RestRequest(apiUrl + baseRequest.Route, baseRequest.Method);

                request.AddHeader("Content-Type", baseRequest.ContentType);

                request.AddHeader("X-Sign", "1_GEM123456");



                if (baseRequest.Parameter != null)


                    request.AddStringBody(JsonConvert.SerializeObject(baseRequest.Parameter), DataFormat.Json);
                //request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);

                var response = await client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)

                    return JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                else
                    return new ApiResponse()
                    {
                        Status = false,
                        Result = null,
                        Message = response.ErrorMessage
                    };
            }
            catch (Exception)
            {

                return null;
            }
            
          
        }

        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(apiUrl + baseRequest.Route);
            request.Method = baseRequest.Method;
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
           
            //client.BaseUrl = new Uri(apiUrl + baseRequest.Route);
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);

            else
                return new ApiResponse()
                {
                    Status = false,
                    Result = null,
                    Message = response.ErrorMessage
                };
        }

        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest(apiUrl + baseRequest.Route);
            request.Method = baseRequest.Method;
            request.AddHeader("Content-Type", baseRequest.ContentType);

            if (baseRequest.Parameter != null)
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
           
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);

            else
                return new ApiResponse<T>()
                {
                    Status = false,
                    Message = response.ErrorMessage
                };
        }
    }
}
