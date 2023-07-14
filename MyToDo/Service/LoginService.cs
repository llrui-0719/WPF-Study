using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class LoginService : ILoginService
    {

        private readonly HttpRestClient httpRestClient;
        private readonly string serviceName = "Login";
        public LoginService(HttpRestClient httpRestClient)
        {
            this.httpRestClient = httpRestClient;
        }

        public async Task<ApiResponse> LoginAsync(UserDto dto)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{serviceName}/Login";
            request.Parameter = dto;
            return await httpRestClient.ExecuteAsync(request);
        }

        public async Task<ApiResponse> RegisterAsync(UserDto dto)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.POST;
            request.Route = $"api/{serviceName}/Register";
            request.Parameter = dto;
            return await httpRestClient.ExecuteAsync(request);
        }
    }
}
