using MyToDo.Common.Models;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class ToDoService:BaseService<ToDoDto>,IToDoService
    {
        private readonly HttpRestClient httpRestClient;

        public ToDoService(HttpRestClient httpRestClient):base(httpRestClient,"ToDo")
        {
            this.httpRestClient = httpRestClient;
        }

        public async Task<ApiResponse<PagedList<ToDoDto>>> GetAllFilterAsync(ToDoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/ToDo/GetAll?pageIndex={parameter.PageIndex}" +
                $"&pageSize={parameter.PageSize}" +
                $"&search={parameter.Search}"+
                $"&status={parameter.Status}";
            return await httpRestClient.ExecuteAsync<PagedList<ToDoDto>>(request);
        }

        public async Task<ApiResponse<SummaryDto>> SummaryAsync()
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.GET;
            request.Route = $"api/ToDo/Summary";
            return await httpRestClient.ExecuteAsync<SummaryDto>(request);
        }
    }
}
