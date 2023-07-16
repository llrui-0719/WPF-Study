using MyToDo.Shared;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class HttpRestClient
    {
        private readonly string apiurl;
        protected readonly RestClient client;

        public HttpRestClient(string apiurl)
        {
            this.apiurl = apiurl;
            client = new RestClient();
        }

        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);
            if (baseRequest.Parameter != null)
            {
                request.AddParameter("param",JsonConvert.SerializeObject(baseRequest.Parameter),ParameterType.RequestBody);
            }

            client.BaseUrl = new Uri(apiurl + baseRequest.Route);
            var response= await client.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
        }

        public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest(baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);
            if (baseRequest.Parameter != null)
            {
                request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
            }

            client.BaseUrl = new Uri(apiurl + baseRequest.Route);
            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (JsonConvert.DeserializeObject<ApiResponse>(response.Content).Status)
                {
                    return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
                }
                else
                {
                    return new ApiResponse<T>()
                    {
                        Status = false,
                        Message = JsonConvert.DeserializeObject<ApiResponse>(response.Content).Result.ToString(),
                    };
                }
            }
            else
            {
                return new ApiResponse<T>() {
                    Status=false,
                    Message=response.ErrorMessage,
                };
            }
        }
    }
}
