using MyToDo.Common.Models;
using MyToDo.Model;
using MyToDo.Model.Parameter;
using MyToDo.Singletons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class ToDoService:BaseService<ToDo>,IToDoService
    {
        private readonly IFreeSql freeSql = DataBaseConnect.GetFreeSqlInstance();
        public ToDoService():base()
        {
        }

        public async Task<ApiResponse<List<ToDo>>> GetAllFilterAsync(ToDoParameter parameter)
        {
            try
            {
                var select = DataBaseConnect.GetFreeSqlInstance().Select<ToDo>();
                var total = await select.CountAsync();
                var list = await select.Page(parameter.PageIndex, parameter.PageSize).ToListAsync();
                return new ApiResponse<List<ToDo>>()
                {
                    Status = true,
                    Message = "",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ToDo>>() { Status = false, Message = ex.Message };
            }
        }

    }
}
